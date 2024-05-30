using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using xsoft.Data;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;

    public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].ToString().Split(" ").Last();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<DataContext>();

                var authentication = await _context.Authentications.SingleOrDefaultAsync(a => a.Token == token);
                if (authentication != null)
                {
                    var IdClaim = GetIdFromToken(token);
                    var EmailClaim = GetEmailFromToken(token);
                    var RoleClaim = GetRoleFromToken(token);
                    if (IdClaim != null)
                    {
                        var newToken = CreateToken(IdClaim.Value, EmailClaim.Value, RoleClaim.Value);
                        authentication.Token = newToken;
                        _context.Authentications.Update(authentication);
                        await _context.SaveChangesAsync();

                        context.Response.Headers.Add("refreshedToken", newToken);
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Invalid token.");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid or expired token.");
                    return;
                }
            }
        }

        await _next(context);
    }

    private Claim GetIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
        var key = Encoding.UTF8.GetBytes(appSettingsToken);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal.FindFirst(ClaimTypes.NameIdentifier);
        }
        catch
        {
            return null;
        }
    }

    private Claim GetEmailFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
        var key = Encoding.UTF8.GetBytes(appSettingsToken);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal.FindFirst(ClaimTypes.Email);
        }
        catch
        {
            return null;
        }
    }

    private Claim GetRoleFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
        var key = Encoding.UTF8.GetBytes(appSettingsToken);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal.FindFirst(ClaimTypes.Role);
        }
        catch
        {
            return null;
        }
    }

    private string CreateToken(string userId, string email, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
        if (appSettingsToken == null)
            throw new Exception("AppSettings Token is null");

        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

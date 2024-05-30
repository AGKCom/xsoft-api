namespace xsoft.models
{
    public class Authentication
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string IdentityJson { get; set; } //email,role serilized as json
        public DateTime ExpiresAt { get; set; }  // Token expiration time

    }
}

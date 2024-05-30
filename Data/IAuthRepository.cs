using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xsoft.models;

namespace xsoft
{
    public interface IAuthRepository 
    {
            Task<ServiceResponse<int>> Register(User user, string password);
            Task<ServiceResponse<int>> RegisterAdmin(Admin admin, string password);
            Task<ServiceResponse<string>> Login(string email, string password);
            Task<ServiceResponse<string>> LoginAdmin(string email, string password);
            Task<bool> UserExists(string email);
            Task<bool> AdminExists(string email);
    }
}
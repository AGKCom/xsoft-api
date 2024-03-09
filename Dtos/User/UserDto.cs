using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xsoft.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string email { get; set; }=String.Empty;
        public string phone { get; set; }=String.Empty;
        public DateTime expirationDate { get; set; }

    }
}
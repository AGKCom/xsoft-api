using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xsoft.models
{
    public class Configuration
    {
        public int Id { get; set; }
        public string OrganisationName { get; set; }
        public string DbHost { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
        public int MaxUsers { get; set; }
        public DateTime experationDate { get; set; }
        public List<UserConfiguration> UserIds { get; set; } 
        public string GetConnectionString()
            {
                return $"Server={this.DbHost};Database={this.DbName};User Id={this.DbUser};Password={this.DbPassword};TrustServerCertificate=True;";
            }
    
    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xsoft.models
{
    public class Configuration
    {
        public int id { get; set; }
        public string companyName { get; set; } = String.Empty;
        public string dbHost { get; set; } = String.Empty;
        public string database { get; set; } = String.Empty;
        public string dbUser { get; set; } = String.Empty;
        public string dbPassword { get; set; } = String.Empty;
        public User owner { get; set; }
        public int ownerId { get; set; }
        public List<User> collaborators { get; set; }
        public string GetConnectionString()
        {
            return $"Server={this.dbHost};Database={this.database};User Id={this.dbUser};Password={this.dbPassword};TrustServerCertificate=True;";
        }
    }
}
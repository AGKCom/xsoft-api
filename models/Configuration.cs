using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xsoft.models
{
    public class Configuration
    {
        public int Id { get; set; }
        public string companyName { get; set; } = String.Empty;
        public string dbHost { get; set; } = String.Empty;
        public string database { get; set; } = String.Empty;
        public string dbUser { get; set; } = String.Empty;
        public string dbPassword { get; set; } = String.Empty;
        public List<UserConfiguration> UserConfigurations { get; set; }=new List<UserConfiguration>();
    }
}
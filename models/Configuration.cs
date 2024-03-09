using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xsoft.models
{
    public class Configuration
    {
        public int Id { get; set; }
        public string connectionString { get; set; }=String.Empty;
        public List<UserConfiguration> UserConfigurations { get; set; }=new List<UserConfiguration>();
    }
}
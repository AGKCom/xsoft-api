using System;
using xsoft.models;

namespace xsoft.models
{
    public class UserConfiguration
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ConfigurationId { get; set; }
        public Configuration Configuration { get; set; }
    }
}

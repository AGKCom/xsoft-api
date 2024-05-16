using xsoft.models;

namespace xsoft
{
    public class UserConfiguration
    {
        public int UserId { get; set; }
        public User User { get; set; } // Navigation property
        public int ConfigurationId { get; set; }
        public Configuration Configuration { get; set; } // Navigation property
    }

}

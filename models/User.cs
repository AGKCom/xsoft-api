using Microsoft.AspNetCore.Identity;
using xsoft.models;

namespace xsoft
{
    public enum TYPE
    {
        OWNER,
        COLLABORATOR
    }
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int ConfigurationId { get; set; }
        public Configuration Configuration { get; set; } // Navigation property
        public TYPE Type { get; set; }
    }

}

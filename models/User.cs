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
        public bool isActive { get; set; } = false;
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public TYPE Type { get; set; }
        public int? profileId { get; set; }=null;
        public Profile? profile { get; set; } // Navigation property
    }

}

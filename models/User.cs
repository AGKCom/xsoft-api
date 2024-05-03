using Microsoft.AspNetCore.Identity;
using xsoft.models;

namespace xsoft
{
    public class User:Admin
    {
        public int maxCollaborators { get; set; } = 0;
        public string organization { get; set; } = String.Empty;
        public string phone { get; set; } = String.Empty;
        public bool isConfirmed { get; set; } = false;
        public int roleId { get; set; }
        public Role role { get; set; }
        public List<OverridedPermission> userPermissions { get; set; }
        public DateTime expirationDate { get; set; } = DateTime.Now + TimeSpan.FromDays(30);

        // Navigation properties
        public Configuration OwnedConfiguration { get; set; } // Configurations owned by the user
        public List<Configuration> CollaboratingConfigurations { get; set; } = new List<Configuration>(); // Configurations where the user is a collaborator

    }
}

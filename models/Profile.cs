using xsoft.models;

namespace xsoft
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProfilePermission> ProfilePermissions { get; set; } // Navigation property
    }

}

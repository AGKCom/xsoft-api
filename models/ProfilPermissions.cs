namespace xsoft.models
{
    public class ProfilePermission
    {
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } // Navigation property
        public int ProfileId { get; set; }
        public Profile Profile { get; set; } // Navigation property
    }

}

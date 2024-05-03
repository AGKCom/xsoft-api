using xsoft.models;

namespace xsoft
{
    public class OverridedPermission
    {
        public int Id { get; set; }
        public int configurationId { get; set; }
        public Configuration? configuration { get; set; }
        public int userId { get; set; }
        public User? user { get; set; }
        public int permissionId { get; set; }
        public Permission? permission { get; set; }
        public bool isGranted { get; set; }
    }
}

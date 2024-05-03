using xsoft.models;

namespace xsoft
{
    public class Permission
    {
        public int id { get; set; }
        public string name { get; set; } = String.Empty;
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}

using xsoft.models;

namespace xsoft
{
    public class Role
    {
        public int id { get; set; }
        public string name { get; set; } = String.Empty;
        public List<User> Users { get; set; } = new List<User>();
        public List<Permission> permissions { get; set; }
    }
}

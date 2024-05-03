using xsoft.models;

namespace xsoft
{
    public class Admin
    {
        public int id { get; set; }
        public string email { get; set; } = String.Empty;
        public byte[] passwordhash { get; set; } = new byte[0];
        public byte[] passwordSalt { get; set; } = new byte[0];
    }
}

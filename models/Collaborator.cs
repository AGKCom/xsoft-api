using System;
using xsoft.models;

namespace xsoft
{
    public class Collaborator
    {
        public int id { get; set; }
        public string name { get; set; }=String.Empty;
        public string email { get; set; } = String.Empty;
        public byte[] passwordhash { get; set; } = new byte[0];
        public byte[] passwordSalt { get; set; } = new byte[0];
        public bool isConfirmed { get; set; } = false;
        public Client client { get; set; }
        public int clientId { get; set; }

    }
}

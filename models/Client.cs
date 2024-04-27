using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xsoft.models;

namespace xsoft
{
    public class Client
    {
        public int id { get; set; }
        public string email { get; set; }=String.Empty;
        public int maxCollaborators { get; set; } = 0;
        public string organization { get; set; }=String.Empty;
        public byte[] passwordhash { get; set; }=new byte[0];
        public byte[] passwordSalt { get; set; }=new byte[0];
        public string phone { get; set; }=String.Empty;
        public bool isConfirmed { get; set; } = false;
        public List<Configuration> Configurations { get; set; } = new List<Configuration>();
        public List<Collaborator> collaborators { get; set; } = new List<Collaborator>();
        public DateTime expirationDate { get; set; } = DateTime.Now+TimeSpan.FromDays(30);
    }
}
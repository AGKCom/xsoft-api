using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xsoft.models;

namespace xsoft
{
    public class User
    {
        public int id { get; set; }
        public string email { get; set; }=String.Empty;
        public byte[] passwordhash { get; set; }=new byte[0];
        public byte[] passwordSalt { get; set; }=new byte[0];
        public string phone { get; set; }=String.Empty;
        public List<Configuration>? configurations {get; set;}
        public DateTime expirationDate { get; set; }
    }
}
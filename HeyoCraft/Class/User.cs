using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyoCraft.Class
{
    public class User
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public string UUID { get; set; }

        public string SID { get; set; }
        public string accessToken { get; set; }

        public bool Suc { get; set; }

        public string Errinfo { get; set; }

        public string OtherInfo { get; set; }

        public string Client_identifier { get; set; }

        public string OutInfo { get; set; }
    }
}

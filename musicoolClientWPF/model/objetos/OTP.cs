using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicoolClientWPF.model.objetos
{
    internal class OTP
    {
        public string Username { get; set; }
        public string Codigo { get; set; }
        public DateTime expiracion { get; set;}
    }
}

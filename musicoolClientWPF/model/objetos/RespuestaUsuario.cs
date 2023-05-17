using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicoolClientWPF.model.objetos
{
    internal class RespuestaUsuario
    {
        [JsonProperty("access_token")]
        public string access_token { get; set; }
        public Usuario _Usuario { get; set;}
        public OTP _Otp { get; set;}
    }
}

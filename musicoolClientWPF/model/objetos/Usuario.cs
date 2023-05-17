using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicoolClientWPF.model.objetos
{
    public class Usuario
    {
        [JsonProperty("username")]

        public string username { get; set; }
        [JsonProperty("password")]

        public string password { get; set; }

        [JsonProperty("telefono")]

        public string telefono { get; set; }

        public string Otp { get; set; }

    }
}

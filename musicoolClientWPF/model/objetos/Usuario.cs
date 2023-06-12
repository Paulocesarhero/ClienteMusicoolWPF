using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

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

        [JsonProperty("access_token")]

        public string acces_token { get; set; }

        [JsonProperty ("token_type")]

        public string token_type { get; set; }

        public string Otp { get; set; }

        public string rol { get; set; }

    }
}

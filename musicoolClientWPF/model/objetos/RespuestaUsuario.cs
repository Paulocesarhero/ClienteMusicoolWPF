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
        [JsonProperty("token_type")]
        public string token_type { get; set; }
        [JsonProperty("rol")]
        public string rol { get; set;}

    }
}

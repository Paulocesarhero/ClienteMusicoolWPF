using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace musicoolClientWPF.model.objetos
{
    public class CancionRespuesta
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("nombre")]
        public string nombre { get; set; }

        [JsonProperty("artista")]
        public string artista { get; set; }

        [JsonProperty("fechaD")]
        public string fechaDePublicacion { get; set; }
    }
}
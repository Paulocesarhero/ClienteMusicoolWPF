using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using musicoolClientWPF.model.objetos;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

namespace musicoolClientWPF.model.api
{
    internal class UsuarioServices
    {
        private string _UrlBase { get; set; }

        public UsuarioServices()
        {
            getSettings();
        }

        private void getSettings()
        {
            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string json = r.ReadToEnd();
                dynamic data = JObject.Parse(json);
                _UrlBase = data.configApi.urlBase;
            }
        }
        public async Task<RespuestaUsuario> getTokenAcceso(Usuario usuario)
        {
            RespuestaUsuario respuestaUsuario = new RespuestaUsuario();
            string urlBase = _UrlBase;
            urlBase += "token";
            var httpClient = new HttpClient();
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", usuario.username),
                new KeyValuePair<string, string>("password", usuario.password)
            };
            var request = new HttpRequestMessage(HttpMethod.Post, urlBase);
            request.Content = new FormUrlEncodedContent(keyValues);
            try
            {
                var response = await httpClient.SendAsync(request);
                if (response.EnsureSuccessStatusCode() != null)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    respuestaUsuario = JsonConvert.DeserializeObject<RespuestaUsuario>(responseBody);
                    respuestaUsuario.Error = false;
                    respuestaUsuario.Mensaje = "OK";
                    respuestaUsuario._Usuario = usuario;
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                respuestaUsuario.Error = true;
                respuestaUsuario.Mensaje = "Contraseña o nombre de usuario incorrecto";
            }
            catch (HttpRequestException e) 
            {
                throw new Exception("Error en la conexión con el servidor", e);
            }
            return respuestaUsuario;
        }
    }
}

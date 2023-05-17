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
using System.Net.Http.Headers;

namespace musicoolClientWPF.model.api
{
    internal class UsuarioServices
    {
        private string _UrlBase { get; set; }
        private BasicAuth _BasicAuth { get; set; } = new BasicAuth();

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
                _BasicAuth.Cliente = data.configApi.cliente;
                _BasicAuth.Contra = data.configApi.contra;
            }
        }
        public async Task<RespuestaUsuario> EnviarToken(Usuario usuario)
        {
            RespuestaUsuario respuestaUsuario = new RespuestaUsuario();
            string urlBase = _UrlBase;
            urlBase += "login";
            var httpClient = new HttpClient();
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", usuario.username),
                new KeyValuePair<string, string>("password", usuario.password)
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, urlBase);
            request.Content = new FormUrlEncodedContent(keyValues);
            autentificacionBasica(ref httpClient);
            try
            {
                var response = await httpClient.SendAsync(request);
                if (response.EnsureSuccessStatusCode() != null)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    respuestaUsuario = JsonConvert.DeserializeObject<RespuestaUsuario>(responseBody);
                    respuestaUsuario._Usuario = usuario;
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Contraseña o nombre uncorrectos", ex);
            }
            catch (HttpRequestException e) 
            {
                throw new Exception("Error en la conexión con el servidor", e);
            }
            return respuestaUsuario;
         }

        public async Task<bool> ValidarOtp(Usuario usuario)
        {
            RespuestaUsuario respuestaUsuario = new RespuestaUsuario();
            bool bandera = false;
            string urlBase = _UrlBase;
            urlBase += "login/auth";
            var httpClient = new HttpClient();
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", usuario.username),
                new KeyValuePair<string, string>("password", usuario.Otp)
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
                    respuestaUsuario._Usuario = usuario;
                    bandera = true;
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Contraseña o nombre uncorrectos", ex);
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Error en la conexión con el servidor", e);
            }

            return bandera;
        }

        private void autentificacionBasica(ref HttpClient httpC)
        {
            string authInfo = "${_BasicAuth.Cliente}:{_BasicAuth.Contra}";
            authInfo = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authInfo));
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Basic", authInfo);
            httpC.DefaultRequestHeaders.Authorization = authHeader;

        }
    }
}

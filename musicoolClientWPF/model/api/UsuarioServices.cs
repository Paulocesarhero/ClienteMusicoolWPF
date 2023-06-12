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
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using musicoolClientWPF.Utilidades;
using System.Windows;
using System.Windows.Media.Animation;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace musicoolClientWPF.model.api
{
    internal class UsuarioServices
    {
        private string _UrlBase { get; set; }
        private BasicAuth _BasicAuth { get; set; } = new BasicAuth();

        public UsuarioServices()
        {
            _BasicAuth = new BasicAuth()
            {
                Cliente = Configuraciones.GetCliente(),
                Contra = Configuraciones.GetBasicAuthPassword()
            };
            _UrlBase = Configuraciones.GetUrlBase();
        }

        public async Task<bool> EnviarToken(Usuario usuario)
        {
            bool bandera = false;
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
                   
                    MessageBox.Show("Hemos enviado un código a su celular",
                        "Información",
                        MessageBoxButton.OK);
                    bandera = true;
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Contraseña o nombre incorrectos", ex);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw ex;
            }
            catch (HttpRequestException e) 
            {
                throw new Exception("Error en la conexión con el servidor", e);
            }
            return bandera;
         }

        public async Task<bool> ValidarOtp(Usuario usuario)
        {
            RespuestaUsuario respuestaUsuario = new RespuestaUsuario();
            bool bandera = false;
            string urlBase = _UrlBase + "login/auth";
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
                    InfoUsuario.Instance.Usuario.acces_token = respuestaUsuario.access_token;
                    InfoUsuario.Instance.Usuario.token_type = respuestaUsuario.token_type;
                    InfoUsuario.Instance.Usuario.rol = respuestaUsuario.rol;
                    bandera = true;
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Contraseña o nombre incorrectos", ex);
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Error en la conexión con el servidor", e);
            }

            return bandera;
        }

        public async Task<bool> CrearUsuario(Usuario usuario)
        {
            bool bandera = false;
            string urlBase = _UrlBase + "usuarios";
            Dictionary<string, string> requestData = new Dictionary<string, string>
            {
                { "username", usuario.username },
                { "password", usuario.password },
                { "telefono", usuario.telefono },
                { "rol", usuario.rol }
            };

            HttpClient httpClient = new HttpClient();
            

            try
            {
                string jsonContent = JsonSerializer.Serialize(requestData);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                autentificacionBasica(ref httpClient);
                var response = await httpClient.PostAsync(urlBase, content);

                if (response.IsSuccessStatusCode)
                {
                    bandera = true;
                }
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Error en la conexión con el servidor", e);
            }

            return bandera;
        }

        private void autentificacionBasica(ref HttpClient httpC)
        {
            var cliente = _BasicAuth.Cliente;
            var contra = _BasicAuth.Contra;

            string authInfo = cliente + ":" + contra;
            authInfo = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authInfo));
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Basic", authInfo);
            httpC.DefaultRequestHeaders.Authorization = authHeader;

        }
    }
}

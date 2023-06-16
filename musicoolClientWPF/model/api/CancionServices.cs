using musicoolClientWPF.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using musicoolClientWPF.model.objetos;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace musicoolClientWPF.model.api
{
    public class CancionServices
    {
        private string _UrlBase { get; set; }

        public CancionServices()
        {
            _UrlBase = Configuraciones.GetUrlBase();
        }

        public bool SubirCancion(string imgenPath, string songPath, string idCancion)
        {
            string url = _UrlBase + "subir-cancion?cancion_id=" + idCancion;
            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent form = new MultipartFormDataContent();

                // Agregar la imagen
                byte[] imageBytes = File.ReadAllBytes(imgenPath);
                ByteArrayContent imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                form.Add(imageContent, "image", idCancion);

                // Agregar la canción
                byte[] songBytes = File.ReadAllBytes(songPath);
                ByteArrayContent songContent = new ByteArrayContent(songBytes);
                songContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");
                form.Add(songContent, "song", idCancion);

                HttpResponseMessage response = client.PostAsync(url, form).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception("eror al subir cancion " + response.StatusCode);
                }
            }
        }

        public async Task<string> RegistrarCancio(string nombreCancion, string artista)
        {
            string url = _UrlBase + "registrar-cancion";

            using (HttpClient client = new HttpClient())
            {
                // Configurar el encabezado "accept"
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Configurar el encabezado "token"
                client.DefaultRequestHeaders.Add("token", InfoUsuario.Instance.Usuario.acces_token);

                // Crear el objeto JSON para enviar en el cuerpo de la solicitud
                var requestBody = new
                {
                    nombre = nombreCancion,
                    artista = artista,
                    fechaDePublicacion = "Lorem"
                };

                // Realizar la solicitud POST
                HttpResponseMessage response = await client.PostAsJsonAsync(url, requestBody);

                if (response.IsSuccessStatusCode)
                {
                    string resultado = await response.Content.ReadAsStringAsync();
                    string resultadoformateado = resultado.Substring(1, resultado.Length - 2);
                    return resultadoformateado.Trim('"');
                }
                else
                {
                    throw new Exception("eror al registrar cancion " + response.StatusCode);
                }
            }
        }

        public async Task<CancionRespuesta> BuscarCancion(string artista, string nombre)
        {
            string url = _UrlBase + "buscar-cancion";
            string json = JsonSerializer.Serialize(new { id = "", nombre = nombre, artista = artista, fechaDePublicacion = "" });

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("token", InfoUsuario.Instance.Usuario.acces_token);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<CancionRespuesta>(responseJson);
                }
                else
                {
                    return new CancionRespuesta()
                    {
                        artista = "",
                        fechaDePublicacion = "",
                        id = "",
                        nombre = ""
                    };
                }
            }
        }

        public async Task<byte[]> ObtenerImagen(string id)
        {
            string url = _UrlBase + "obtener-imagen?id=" + id;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("token", InfoUsuario.Instance.Usuario.acces_token);

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<string> ObtenerCancion(string id)
        {
            string url = _UrlBase + "obtener-cancion?id=" + id;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("token", InfoUsuario.Instance.Usuario.acces_token);

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string archivoTemporal = Path.ChangeExtension(Path.GetTempFileName(), ".mp4");
                    using (FileStream fileStream = new FileStream(archivoTemporal, FileMode.Create, FileAccess.Write))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }
                    return archivoTemporal;
                }
                else
                {
                    return "";
                }
            }
        }

        public async Task<bool> ComentarCancion(string idCancion, string comentario)
        {
            string url = _UrlBase + "agregar-comentario";

            HttpClient client = new HttpClient();

            // Configura los encabezados
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("token", InfoUsuario.Instance.Usuario.acces_token);

            // Crea el objeto de contenido JSON
            string jsonContent = $@"
    {{
        ""idCancion"": ""{idCancion}"",
        ""autor"": ""{InfoUsuario.Instance.Usuario.username}"",
        ""comentario"": ""{comentario}"",
        ""fechaDeComentario"": ""string""
    }}";

            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Realiza la solicitud POST
            HttpResponseMessage response = await client.PostAsync(url, httpContent);

            // Verifica si la petición fue exitosa
            bool isSuccess = response.IsSuccessStatusCode;

            return isSuccess;
        }

        public async Task<List<Comentario>> ObtenerComentarios(string idCancion)
        {
            string url = _UrlBase + "obtener-comentarios?id_Cancion=" + idCancion;

            HttpClient client = new HttpClient();

            // Configurar los encabezados
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("token", InfoUsuario.Instance.Usuario.acces_token);

            // Realizar la solicitud POST
            HttpResponseMessage response = await client.PostAsync(url, null);

            // Verificar si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserializar la respuesta en una lista de objetos Comentario
                List<Comentario> comentarios = JsonSerializer.Deserialize<List<Comentario>>(responseBody);

                return comentarios;
            }
            else
            {
                Console.WriteLine("Error al obtener los comentarios. Código de respuesta: " + response.StatusCode);
                return null;
            }
        }

        public class Comentario
        {
            [JsonProperty("autor")]
            public string autor { get; set; }

            [JsonProperty("comentario")]
            public string comentario { get; set; }
        }
    }
}
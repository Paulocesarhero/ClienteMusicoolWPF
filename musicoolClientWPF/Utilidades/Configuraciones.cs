using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace musicoolClientWPF.Utilidades
{
    public static class Configuraciones
    {
        public static IConfiguration Configuration { get; set; }

        static Configuraciones()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public static string GetUrlBase()
        {
            return Configuration.GetConnectionString("urlBase");
        }

        public static string GetCliente()
        {
            return Configuration.GetConnectionString("cliente");
        }

        public static string GetBasicAuthPassword()
        {
            return Configuration.GetConnectionString("contra");
        }
    }
}

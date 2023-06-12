using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteGrpc
{
    public class Servicio
    {
        public static Data ObtenerCancion(Cancion cancion)
        {
            var channelOptions = new GrpcChannelOptions
            {
                MaxReceiveMessageSize = 10 * 1024 * 1024 // 10 MB
            };
            var channel = GrpcChannel.ForAddress("http://localhost:50051", channelOptions);
            var client = new Streamer.StreamerClient(channel);
            return  client.Audio(cancion);
        }
    }
}

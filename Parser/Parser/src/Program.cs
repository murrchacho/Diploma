using Parser.connection;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client.Common;

namespace Parser
{
    internal class Program
    {
        async static Task Main(string[] args)
        {
            Uri url = UrlBuilder.Build("CoDeSys.OPC.02");
            TitaniumAS.Opc.Client.Bootstrap.Initialize();
            Initialize initialize = new Initialize();

            IPHostEntry IpHost = Dns.GetHostEntry("localhost");
            IPAddress IpAddr = IpHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(IpAddr, 8080);

            using (SocketConnection server = new SocketConnection(ipEndPoint, SocketType.Stream, ProtocolType.Tcp))
            {
                await server.socket.ConnectAsync(ipEndPoint);
                await initialize.Start(url, server);
            }
        }
    }
}

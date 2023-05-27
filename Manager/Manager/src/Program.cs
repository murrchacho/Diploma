using Manager.connection;
using Manager.db;
using System.Net;
using System.Net.Sockets;

namespace Manager.src
{
    internal class Program
    {
        async static Task Main(string[] args)
        {
            IPHostEntry IpHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = IpHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8080);

            using (ConditionsContext db = new ConditionsContext())
            using (SocketConnection connection = new SocketConnection(ipEndPoint, SocketType.Stream, ProtocolType.Tcp))
            {
                SocketConnection client = await connection.AcceptAsync();
                await Task.Run(async () => await ProcessClientAsync(client, db));
            }
        }
        async static Task ProcessClientAsync(SocketConnection client, ConditionsContext db)
        {
            Console.WriteLine($"Подключение установлено с {client.socket.RemoteEndPoint}");

            try
            {
                while (true)
                {
                    DbActions actions = new DbActions(client, db);
                    string request = await client.ReceiveData();
                    actions.Action(request);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally 
            {
                client.socket.Shutdown(SocketShutdown.Both);
                client.socket.Close();
            }
        }
    }
}

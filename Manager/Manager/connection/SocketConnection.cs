using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Manager.connection
{
    internal class SocketConnection : IDisposable
    {
        public SocketConnection(Socket socket) 
        { 
            this.socket = socket;
        }
        public SocketConnection(IPEndPoint ipEndPoint, SocketType socketType, ProtocolType protocolType)
        {
            this.socket = new Socket(ipEndPoint.AddressFamily, socketType, protocolType);
            this.socket.Bind(ipEndPoint);
            this.socket.Listen();

            Console.WriteLine("Ожидаем входящих подключений..");
        }
        public Socket socket { get; set; }
        public void Dispose()
        {
            this.socket.Dispose();
        }
        public async Task<SocketConnection> AcceptAsync()
        {
            return new SocketConnection(await this.socket.AcceptAsync());
        }
        public async void SendData(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            await socket.SendAsync(buffer, SocketFlags.None);
        }
        public async Task<string> ReceiveData(int size = 13)
        {
            byte[] buffer = new byte[size];
            int bytes = await this.socket.ReceiveAsync(buffer, SocketFlags.None);
            string request = Encoding.UTF8.GetString(buffer, 0, bytes);

            Console.WriteLine($"Получен запрос: '{request}' с хоста: {this.socket.RemoteEndPoint}");

            return request;
        }
    }
}

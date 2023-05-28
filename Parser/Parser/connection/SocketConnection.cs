using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Parser.connection
{
    internal class SocketConnection : IDisposable
    {
        public SocketConnection(Socket socket) 
        { 
            this.socket = socket;
        }
        public SocketConnection(IPEndPoint ipEndPoint, SocketType socketType, ProtocolType protocolType)
        {
            Console.WriteLine("Подключаемся к серверам..");
            this.socket = new Socket(ipEndPoint.AddressFamily, socketType, protocolType);
        }
        public Socket socket { get; set; }
        public OpcGroup opcGroups { get; set; }
        public void Dispose()
        {
            this.socket.Dispose();
        }
        public void SendData(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(buffer, 0, buffer.Length);
            e.Completed += new EventHandler<SocketAsyncEventArgs>(SendDataCallback);

            bool completed = false;

            try
            {
                completed = this.socket.SendAsync(e);
            }
            catch (SocketException exception)
            {
                Console.WriteLine(exception.ErrorCode);
            }

            if (!completed)
            {
                SendDataCallback(this, e);
            }
        }
        public void SendDataCallback(object ServerSocket, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                Console.WriteLine(e.SocketError);
            }
        }
        private void ResetBuffer(SocketAsyncEventArgs e)
        {
            var buffer = new Byte[1024];

            e.SetBuffer(buffer, 0, 1024);
        }
        private void ReceiveDataCallback(Object sender, SocketAsyncEventArgs e)
        {
            ProcessData(e.Buffer, 0, e.BytesTransferred);

            ResetBuffer(e);

            socket.ReceiveAsync(e);
        }
        private void ProcessData(Byte[] data, int v, Int32 count)
        {
            opcGroups.SetConditions(Encoding.UTF8.GetString(data));
        }
        public void ReceiveData(OpcGroup groups, int size = 1024)
        {
            this.opcGroups = groups;

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveDataCallback);

            ResetBuffer(e);

            this.socket.ReceiveAsync(e);
        }
    }
}

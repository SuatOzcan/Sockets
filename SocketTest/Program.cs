using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string server = "localhost";
            int port = 5000;
            string path = "api/values";
            //string path = "/weatherforecast";

            Socket socket = null;
            IPEndPoint endPoint = null;
            var host = Dns.GetHostEntry(server);

            foreach (var address in host.AddressList)
            {
                socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                endPoint = new IPEndPoint(address, port);
                await socket.ConnectAsync(endPoint);
                if (socket.Connected)
                {
                    var message = GetRequestMessage(server, port, path);
                    var messageBytes = Encoding.ASCII.GetBytes(message);
                    var segment = new ArraySegment<byte>(messageBytes);
                    await socket.SendAsync(segment, SocketFlags.None);
                }
            }
            
        }

        private static string GetRequestMessage(string server, int port, string path)
        {
            var message = $"GET {path} HTTP/1.1\r\n";
            message += $"Host: {server}:{port}\r\n";
            message += "cache-control: no-cache\r\n";
            message += "\r\n";
            return message;
        }
    }
}

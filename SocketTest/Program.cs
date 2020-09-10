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
           // string path = "/api/values"; 77this is for dotnet 2.2 framework
           string path = "/weatherforecast"; //this one is for dotnet 3.1 framework

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
                    break;
                }
            }
            var message = GetRequestMessage(server, port, path);
            var messageBytes = Encoding.ASCII.GetBytes(message);
            var segment = new ArraySegment<byte>(messageBytes);
            await socket.SendAsync(segment, SocketFlags.None);
            var receiveSeg = new ArraySegment<byte>(new byte[512],0,512);
            await socket.ReceiveAsync(receiveSeg, SocketFlags.None);
            string receivedMessage = Encoding.UTF8.GetString(receiveSeg);
            foreach (var line in receivedMessage.Split("\r\n"))
            {
                Console.WriteLine(line);
            }
            socket.Disconnect(false);
            socket.Dispose();
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

using System;
using System.Net;
using System.Net.Sockets;
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
                if (socket.Connected)
                {
                    break;
                }
            }
        }
    }
}

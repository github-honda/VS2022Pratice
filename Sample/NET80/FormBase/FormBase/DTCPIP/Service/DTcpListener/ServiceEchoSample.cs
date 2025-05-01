using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Service.DTcpListener
{
    internal class ServiceEchoSample
    {
        public async Task Listen()
        {
            Console.WriteLine("Server 啟動中...");

            TcpListener listener = new TcpListener(IPAddress.Any, 11000);
            listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }
        private async Task HandleClientAsync(TcpClient client)
        {
            Console.WriteLine($"Client 連線: {client.Client.RemoteEndPoint}");
            var stream = client.GetStream();
            var buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int length = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (length == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, length);
                    Console.WriteLine($"收到: {message}");

                    string response = "Server 收到: " + message;
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"連線錯誤: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"Client 離線: {client.Client.RemoteEndPoint}");
                client.Close();
            }
        }

    }
}

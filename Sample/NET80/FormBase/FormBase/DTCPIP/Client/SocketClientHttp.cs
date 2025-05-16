using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace FormBase.DTCPIP.Client
{
    public class SocketClientHttp
    {
        private static readonly string ServerAddress = "127.0.0.1";
        private static readonly int ServerPort = 8080;

        public static async Task RunClientAsync()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerAddress), ServerPort));
                Console.WriteLine("已連接到 HTTP 伺服器");

                using (NetworkStream networkStream = new NetworkStream(clientSocket, true))
                using (StreamWriter writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true })
                using (StreamReader reader = new StreamReader(networkStream, Encoding.UTF8))
                {
                    await writer.WriteLineAsync("GET / HTTP/1.1\r\nHost: localhost\r\n\r\n");

                    string response = await reader.ReadToEndAsync();
                    Console.WriteLine($"收到伺服器回應:\n{response}");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket 錯誤: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO 錯誤: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"客戶端發生錯誤: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }

    }
}

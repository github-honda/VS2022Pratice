/*

usage: 
in main()
    await HttpClientTester.RunTestsAsync();
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace FormBase.DTCPIP.Client
{
    public class SocketClientHttps_Test
    {
        private static readonly string ServerAddress = "127.0.0.1";
        private static readonly int ServerPort = 443;
        private static readonly string[] Methods = { "GET", "HEAD", "POST", "PUT", "DELETE" };

        public static async Task RunTestsAsync()
        {
            foreach (string method in Methods)
            {
                Console.WriteLine($"\n===== 測試 {method} 方法 =====");
                await TestHttpMethodAsync(method);
            }
        }

        private static async Task TestHttpMethodAsync(string method)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerAddress), ServerPort));
                Console.WriteLine($"已連接到伺服器，測試 {method} 方法");

                using (NetworkStream networkStream = new NetworkStream(clientSocket, true))
                using (SslStream sslStream = new SslStream(networkStream, false))
                using (StreamWriter writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true })
                using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                {
                    await sslStream.AuthenticateAsClientAsync("localhost");

                    string request = method == "POST" || method == "PUT"
                        ? $"{method} / HTTP/1.1\r\nHost: localhost\r\nContent-Length: 14\r\n\r\nTest Data Here"
                        : $"{method} / HTTP/1.1\r\nHost: localhost\r\n\r\n";

                    await writer.WriteLineAsync(request);

                    string response = await reader.ReadToEndAsync();
                    Console.WriteLine($"收到 {method} 回應:\n{response}");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket 錯誤: {ex.Message}");
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine($"身份驗證失敗: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO 錯誤: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{method} 測試失敗: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }
    }
}

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
    public class SocketClientHttps
    {
        public static async Task RunClientAsync(string method)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 443));
                Console.WriteLine("已連接至 HTTPS 伺服器");

                using (NetworkStream networkStream = new NetworkStream(clientSocket, true))
                using (SslStream sslStream = new SslStream(networkStream, false))
                using (StreamWriter writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true })
                using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                {
                    await sslStream.AuthenticateAsClientAsync("localhost");

                    string request = $"{method} / HTTP/1.1\r\nHost: localhost\r\n\r\n";
                    await writer.WriteLineAsync(request);

                    string response = await reader.ReadToEndAsync();
                    Console.WriteLine($"收到伺服器回應:\n{response}");
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
                Console.WriteLine($"客戶端發生錯誤: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }
    }
}

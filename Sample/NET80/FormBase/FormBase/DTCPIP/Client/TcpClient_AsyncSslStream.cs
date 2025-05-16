/*

class Program
{
    static async Task Main(string[] args)
    {
        await AsyncSocketClientTemplate.RunClientAsync();
    }
}

主要功能：
- 建立 TCP 連線：使用 TcpClient 來與伺服器建立連線。
- SSL/TLS 加密：使用 SslStream 來確保傳輸安全。
- 伺服器憑證驗證：ValidateServerCertificate 方法可用於驗證伺服器的 SSL 憑證。
- 傳輸加密資料：透過 SslStream.WriteAsync 和 SslStream.ReadAsync 來傳送與接收加密資料。
部署：
- 確保伺服器與客戶端都使用正確的 SSL 憑證 (server-cert.pfx)。
- 執行伺服器後，再運行客戶端以進行安全通訊。
這樣，伺服器和客戶端就可以透過 SSL/TLS 安全地交換資料！


 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Client
{
    public class TcpClient_AsyncSslStream
    {
        private static readonly string ServerAddress = "127.0.0.1"; // 伺服器 IP
        private static readonly int ServerPort = 8080;

        public static async Task RunClientAsync()
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(ServerAddress, ServerPort);
                    Console.WriteLine("已連接到伺服器");

                    using (NetworkStream networkStream = client.GetStream())
                    using (SslStream sslStream = new SslStream(networkStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate)))
                    {
                        await sslStream.AuthenticateAsClientAsync(ServerAddress);

                        // 傳送訊息到伺服器
                        string message = "Hello, Secure Server!";
                        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                        await sslStream.WriteAsync(messageBytes, 0, messageBytes.Length);
                        await sslStream.FlushAsync();
                        Console.WriteLine("已傳送加密訊息到伺服器");

                        // 接收伺服器回應
                        byte[] buffer = new byte[1024];
                        int receivedBytes = await sslStream.ReadAsync(buffer, 0, buffer.Length);
                        string serverMessage = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                        Console.WriteLine($"收到伺服器回應: {serverMessage}");
                    }
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
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            Console.WriteLine($"SSL 憑證驗證錯誤: {sslPolicyErrors}");
            return false;
        }
    }
}

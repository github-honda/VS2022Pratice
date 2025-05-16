using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Client
{
    public class SocketClientBuffer
    {
        private static readonly string ServerAddress = "127.0.0.1";
        private static readonly int ServerPort = 8080;
        private static readonly X509Certificate2 trustedCertificate = new X509Certificate2("trusted-cert.pfx");

        public static async Task RunClientAsync()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerAddress), ServerPort));
                Console.WriteLine("已連接到伺服器");

                using (NetworkStream networkStream = new NetworkStream(clientSocket, true))
                using (SslStream sslStream = new SslStream(networkStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate)))
                {
                    await sslStream.AuthenticateAsClientAsync(ServerAddress);

                    // 發送加密訊息到伺服器
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

        private static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            Console.WriteLine($"SSL 憑證驗證錯誤: {sslPolicyErrors}");

            if (certificate is X509Certificate2 cert2 && cert2.Equals(trustedCertificate))
            {
                Console.WriteLine("憑證與可信憑證匹配，允許連線");
                return true;
            }

            return false;
        }

    }
}

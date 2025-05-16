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

namespace FormBase.DTCPIP.Service
{
    public class AsycSocketServiceTemplate
    {
        private X509Certificate2 serverCertificate = new X509Certificate2("server-cert.pfx", "password"); // 更換為你的憑證

        public async Task RunServerAsync(int port, CancellationToken cancellationToken)
        {
            try
            {
                using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                    serverSocket.Listen(10);

                    Console.WriteLine($"伺服器已啟動，正在監聽連接 (端口: {port})...");

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        Socket clientSocket = await serverSocket.AcceptAsync();
                        _ = HandleClientAsync(clientSocket, cancellationToken);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO 錯誤: {ex.Message}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket 錯誤: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"伺服器發生錯誤: {ex.Message}");
            }
        }

        public async Task HandleClientAsync(Socket clientSocket, CancellationToken cancellationToken)
        {
            try
            {
                using (NetworkStream networkStream = new NetworkStream(clientSocket, true))
                using (SslStream sslStream = new SslStream(networkStream, false))
                {
                    await sslStream.AuthenticateAsServerAsync(serverCertificate, true, SslProtocols.Tls13, false);

                    byte[] buffer = new byte[1024];
                    int receivedBytes = await sslStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    string clientMessage = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    Console.WriteLine($"收到客戶端訊息: {clientMessage}");

                    byte[] responseBytes = Encoding.UTF8.GetBytes("Hello, Secure Client!");
                    await sslStream.WriteAsync(responseBytes, 0, responseBytes.Length, cancellationToken);
                    await sslStream.FlushAsync(cancellationToken);
                    Console.WriteLine("已回應加密訊息給客戶端");
                }
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine($"身份驗證失敗: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO 錯誤: {ex.Message}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket 錯誤: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }
    }
}

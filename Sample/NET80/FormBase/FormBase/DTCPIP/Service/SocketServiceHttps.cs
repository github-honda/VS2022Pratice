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
    public class SocketServiceHttps
    {
        private X509Certificate2 serverCertificate = new X509Certificate2("server-cert.pfx", "password");

        public async Task RunServerAsync(int port, CancellationToken cancellationToken)
        {
            try
            {
                using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                    serverSocket.Listen(10);

                    Console.WriteLine($"HTTPS 伺服器啟動中 (端口: {port})...");

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        Socket clientSocket = await serverSocket.AcceptAsync();
                        _ = HandleClientAsync(clientSocket);
                    }
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
            catch (Exception ex)
            {
                Console.WriteLine($"伺服器錯誤: {ex.Message}");
            }
        }

        private async Task HandleClientAsync(Socket clientSocket)
        {
            try
            {
                using (NetworkStream networkStream = new NetworkStream(clientSocket, true))
                using (SslStream sslStream = new SslStream(networkStream, false))
                {
                    await sslStream.AuthenticateAsServerAsync(serverCertificate, true, SslProtocols.Tls13, false);

                    using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                    using (StreamWriter writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true })
                    {
                        string? requestLine = await reader.ReadLineAsync();
                        Console.WriteLine($"收到請求: {requestLine}");

                        if (requestLine == null)
                            throw new InvalidDataException("requestLine == null");
                        if (requestLine.StartsWith("GET"))
                            await writer.WriteAsync("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nGET Response!");
                        else if (requestLine.StartsWith("HEAD"))
                            await writer.WriteAsync("HTTP/1.1 200 OK\r\n\r\n");
                        else if (requestLine.StartsWith("POST"))
                            await writer.WriteAsync("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nPOST Response!");
                        else if (requestLine.StartsWith("PUT"))
                            await writer.WriteAsync("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nPUT Response!");
                        else if (requestLine.StartsWith("DELETE"))
                            await writer.WriteAsync("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nDELETE Response!");
                        else
                            await writer.WriteAsync("HTTP/1.1 400 Bad Request\r\nContent-Type: text/plain\r\n\r\nUnsupported Method!");
                    }
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
            catch (Exception ex)
            {
                Console.WriteLine($"處理客戶端錯誤: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }

    }
}

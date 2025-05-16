using FormBase.DItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Service
{
    public class SocketServiceJson
    {
        private X509Certificate2 serverCertificate = new X509Certificate2("server-cert.pfx", "password");
        private DoubleValues? dataStore = new DoubleValues();

        public async Task RunServerAsync(int port, CancellationToken cancellationToken)
        {
            try
            {
                using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                    serverSocket.Listen(10);

                    Console.WriteLine($"伺服器已啟動，正在監聽端口 {port}...");

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        Socket clientSocket = await serverSocket.AcceptAsync();
                        _ = HandleClientAsync(clientSocket);
                    }
                }
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
                        string? requestType = await reader.ReadLineAsync();
                        Console.WriteLine($"收到請求: {requestType}");

                        if (requestType == "UPLOAD")
                        {
                            string jsonData = await reader.ReadToEndAsync();
                            dataStore = JsonSerializer.Deserialize<DoubleValues>(jsonData);
                            await writer.WriteAsync("UPLOAD SUCCESS");
                        }
                        else if (requestType == "RECEIVE")
                        {
                            string responseJson = JsonSerializer.Serialize(dataStore);
                            await writer.WriteAsync(responseJson);
                        }
                        else
                        {
                            await writer.WriteAsync("ERROR: Unsupported Request");
                        }
                    }
                }
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

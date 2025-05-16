using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace FormBase.DTCPIP.Service
{
    public class SocketServiceHttp
    {
        public async Task RunServerAsync(int port, CancellationToken cancellationToken)
        {
            try
            {
                using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                    serverSocket.Listen(10);

                    Console.WriteLine($"HTTP 伺服器已啟動，正在監聽端口: {port}...");

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        Socket clientSocket = await serverSocket.AcceptAsync();
                        _ = HandleClientAsync(clientSocket);
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

        private async Task HandleClientAsync(Socket clientSocket)
        {
            try
            {
                using (NetworkStream networkStream = new NetworkStream(clientSocket, true))
                using (StreamReader reader = new StreamReader(networkStream, Encoding.UTF8))
                using (StreamWriter writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true })
                {
                    string? requestLine = await reader.ReadLineAsync();
                    Console.WriteLine($"收到請求: {requestLine}");

                    string response = "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nHello, HTTP Client!";
                    await writer.WriteAsync(response);
                    Console.WriteLine("已回應 HTTP 訊息");
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
                Console.WriteLine($"處理客戶端錯誤: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }

    }
}

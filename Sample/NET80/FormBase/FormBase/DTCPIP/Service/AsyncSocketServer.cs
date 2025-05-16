/*

#### AsyncSocketServer.cs

1️⃣ 【C# async/await 非同步版 Socket Server】

🧠 async/await server 特點：
連線時用 await AcceptAsync()
收資料用 await ReceiveAsync()
傳資料用 await SendAsync()
每個 client 用一個 Task（非 Thread），大量 client 也不會塞爆 CPU
超適合高併發測試！
*/

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
    internal class AsyncSocketServer
    {
        public async Task Run()
        {
            string localIp = "127.0.0.1";
            int localPort = 11000;

            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(localIp), localPort));
            serverSocket.Listen(100);
            Console.WriteLine($"伺服器啟動，監聽 {localIp}:{localPort}");

            while (true)
            {
                Console.WriteLine("等待客戶端連線...");
                Socket clientSocket = await serverSocket.AcceptAsync();
                Console.WriteLine($"新的客戶端已連線：{clientSocket.RemoteEndPoint}");

                // 開一個新的 Task 非同步處理這個 client
                _ = HandleClientAsync(clientSocket);
            }
        }
        async Task HandleClientAsync(Socket clientSocket)
        {
            var buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int receivedLength = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                    if (receivedLength == 0)
                    {
                        Console.WriteLine($"客戶端 {clientSocket.RemoteEndPoint} 已斷線。");
                        break;
                    }

                    string clientMessage = Encoding.UTF8.GetString(buffer, 0, receivedLength);
                    Console.WriteLine($"收到 {clientSocket.RemoteEndPoint} 的訊息: {clientMessage}");

                    // 回傳訊息
                    string response = $"Server 收到: {clientMessage}";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    await clientSocket.SendAsync(responseBytes, SocketFlags.None);
                    Console.WriteLine($"已回應 {clientSocket.RemoteEndPoint}");
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
                Console.WriteLine($"與客戶端 {clientSocket.RemoteEndPoint} 通訊發生錯誤: {ex.Message}");
            }
            finally
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }

    }
}

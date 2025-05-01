/*

#### AsyncSocketServer.cs

1️⃣ 【C# async/await 非同步版 Socket Server】

🧠 async/await server 特點：
連線時用 await AcceptAsync()
收資料用 await ReceiveAsync()
傳資料用 await SendAsync()
每個 client 用一個 Task（非 Thread），大量 client 也不會塞爆 CPU
超適合高併發測試！


此範例的架構分為以下幾個函數：
- RunServerAsync: 啟動伺服器，監聽連接。
- HandleClientAsync: 處理每一個客戶端的連線與通信。
- Main(): 呼叫伺服器啟動函數 RunServerAsync。

伺服器會等待客戶端的連線請求，接收訊息後回應簡單的文字訊息。
每個客戶端的連線會由 HandleClientAsync 函數非同步處理，確保伺服器能同時處理多個連接。

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Service
{
    public class AsyncSocketServerTemplate
    {
        public async Task Run(string[] args)
        {
            int port = 8080; // 伺服器監聽的端口號
            await RunServerAsync(port);
        }

        public async Task RunServerAsync(int port)
        {
            try
            {
                using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                    serverSocket.Listen(10);

                    Console.WriteLine($"伺服器已啟動，正在監聽連接 (端口: {port})...");

                    while (true)
                    {
                        Socket clientSocket = await serverSocket.AcceptAsync();
                        _ = HandleClientAsync(clientSocket); // 處理客戶端連線 (非同步執行)
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"伺服器發生錯誤: {ex.Message}");
            }
        }

        public async Task HandleClientAsync(Socket clientSocket)
        {
            try
            {
                Console.WriteLine("客戶端已連接");

                // 接收客戶端訊息
                byte[] buffer = new byte[1024];
                int receivedBytes = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                string clientMessage = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                Console.WriteLine($"收到客戶端訊息: {clientMessage}");

                // 傳送回應訊息給客戶端
                string response = "Hello, Client!";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await clientSocket.SendAsync(responseBytes, SocketFlags.None);
                Console.WriteLine("已回應訊息給客戶端");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"處理客戶端時發生錯誤: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }

    }
}

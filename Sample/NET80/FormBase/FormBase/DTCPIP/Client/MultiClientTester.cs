/*

MultiClientTester.cs

2️⃣ 【多 client 測試小程式 (開很多個 client 同時連 server)】
📋 測試流程
啟動 Server（async/await 版）。

啟動 MultiClientTester，輸入想要開幾個 Client（例如 10 個或 100 個）。

每個 client 都會連上 Server 並送 5 次訊息。

Server 每次都即時回應。



*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Client
{
    internal class MultiClientTester
    {
        public async Task Run(string[] args)
        {
            //Console.Write("要啟動幾個 Client？：");
            //int clientCount = int.Parse(Console.ReadLine());
            int clientCount = 10;

            string serverIp = "127.0.0.1";
            int serverPort = 11000;

            var tasks = new Task[clientCount];

            for (int i = 0; i < clientCount; i++)
            {
                int clientId = i + 1;
                tasks[i] = Task.Run(async () =>
                {
                    await StartClientAsync(serverIp, serverPort, clientId);
                });
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("所有 Client 測試完成。按任意鍵離開。");
            Console.ReadKey();
        }

        async Task StartClientAsync(string serverIp, int serverPort, int clientId)
        {
            try
            {
                var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
                Console.WriteLine($"Client {clientId} 已連線");

                for (int i = 0; i < 5; i++) // 每個 client 傳 5 次訊息
                {
                    string message = $"Client {clientId} 的訊息 {i + 1}";
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    await clientSocket.SendAsync(messageBytes, SocketFlags.None);

                    byte[] buffer = new byte[1024];
                    int receivedLength = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                    string response = Encoding.UTF8.GetString(buffer, 0, receivedLength);
                    Console.WriteLine($"Client {clientId} 收到回應: {response}");

                    await Task.Delay(500); // 模擬 client 每0.5秒傳一次
                }

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client {clientId} 發生錯誤: {ex.Message}");
            }
        }

    }
}

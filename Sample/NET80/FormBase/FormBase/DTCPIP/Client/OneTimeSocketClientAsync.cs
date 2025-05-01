using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Client
{
    internal class OneTimeSocketClientAsync
    {
        public async Task Run() 
        {
            string serverIp = "127.0.0.1"; // 請修改為目標伺服器的 IP 地址
            int serverPort = 8080; // 請修改為目標伺服器的端口號

            await RunClientAsync(serverIp, serverPort);

        }
        static async Task RunClientAsync(string serverIp, int serverPort)
        {
            try
            {
                using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    await ConnectToServer(clientSocket, serverIp, serverPort);
                    await SendMessage(clientSocket, "Hello, Server!");
                    await ReceiveResponse(clientSocket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤: {ex.Message}");
            }
        }

        static async Task ConnectToServer(Socket clientSocket, string serverIp, int serverPort)
        {
            await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
            Console.WriteLine("已連接到伺服器");
        }

        static async Task SendMessage(Socket clientSocket, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await clientSocket.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine($"已傳送訊息: {message}");
        }

        static async Task ReceiveResponse(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            int receivedBytes = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
            string serverResponse = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
            Console.WriteLine($"伺服器回應: {serverResponse}");
        }

    }
}

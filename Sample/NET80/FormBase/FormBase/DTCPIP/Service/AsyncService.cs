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
    internal class AsyncService
    {
        public void Run()
        {
            // 監聽的 IP 和 Port
            string localIp = "127.0.0.1";
            int localPort = 11000;

            try
            {
                // 建立 TCP/IP Socket
                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // 綁定 IP 和 Port
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse(localIp), localPort));
                serverSocket.Listen(100); // 最多允許100個等待中的連線
                Console.WriteLine($"伺服器啟動，監聽 {localIp}:{localPort}");

                while (true)
                {
                    Console.WriteLine("等待客戶端連線...");
                    Socket clientSocket = serverSocket.Accept(); // 接受一個連線
                    Console.WriteLine($"新的客戶端已連線，來自 {clientSocket.RemoteEndPoint}");

                    // 為每個 client 啟動一個新的 Thread 處理
                    Thread clientThread = new Thread(() => HandleClient(clientSocket));
                    clientThread.Start();
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
                Console.WriteLine($"伺服器錯誤: {ex.Message}");
            }
            Console.WriteLine("伺服器結束...");
            Console.ReadKey();
        }
        static void HandleClient(Socket clientSocket)
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int receivedLength = clientSocket.Receive(buffer);
                    if (receivedLength == 0)
                    {
                        Console.WriteLine($"客戶端 {clientSocket.RemoteEndPoint} 已斷線。");
                        break;
                    }

                    string clientMessage = Encoding.UTF8.GetString(buffer, 0, receivedLength);
                    Console.WriteLine($"收到 {clientSocket.RemoteEndPoint} 的訊息: {clientMessage}");

                    // 回傳回去
                    string response = $"Server 收到: {clientMessage}";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    clientSocket.Send(responseBytes);
                    Console.WriteLine($"已回應 {clientSocket.RemoteEndPoint}");
                }

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"與客戶端 {clientSocket.RemoteEndPoint} 通訊錯誤: {ex.Message}");
            }
        }
    }
}

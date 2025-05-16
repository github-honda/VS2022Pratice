/*
OncClientService.cs

這個 Server 的流程：
綁定在本機 IP 127.0.0.1，port 11000。
等待客戶端連線。
收到客戶端訊息後，回傳一句話 "Hello from Server!"。
然後關閉該客戶端連線，等待下一個新的客戶端連進來。

這個版本的特點：
一旦有一個 client 連進來，server 會一直在 while (true) 內收資料。
如果 client 主動斷線（Receive 回傳 0），就離開 while 迴圈，關閉該 client 的連線。
每收一筆就「即時回應」給 client。

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
    internal class OneClientService
    {
        public void Run()
        {         // 監聽的 IP 和 Port
            string localIp = "127.0.0.1";
            int localPort = 11000;

            try
            {
                // 建立 TCP/IP Socket
                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // 綁定 IP 和 Port
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse(localIp), localPort));
                serverSocket.Listen(10); // 最多允許 10 個等待中的連線
                Console.WriteLine($"伺服器啟動，監聽 {localIp}:{localPort}");

                while (true)
                {
                    Console.WriteLine("等待客戶端連線...");
                    Socket clientSocket = serverSocket.Accept(); // 接受一個連線
                    Console.WriteLine("客戶端已連線！");

                    // 接收資料
                    byte[] buffer = new byte[1024];
                    int receivedLength = clientSocket.Receive(buffer);
                    string clientMessage = Encoding.UTF8.GetString(buffer, 0, receivedLength);
                    Console.WriteLine("收到客戶端訊息: " + clientMessage);

                    // 回應客戶端
                    string response = "Hello from Server!";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    clientSocket.Send(responseBytes);
                    Console.WriteLine("回傳訊息: " + response);

                    // 關閉與客戶端的連線
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    Console.WriteLine("已關閉與客戶端的連線\n");
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
                Console.WriteLine("發生錯誤: " + ex.Message);
            }

            Console.WriteLine("伺服器結束...");
            Console.ReadKey();
        }

    }
}

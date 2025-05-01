/*
OneTimeSocketClient.cs

One time socket client

說明
Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)：
建立一個 TCP 連線。
Connect：連線到指定的 IP 和 Port。
Send / Receive：分別傳送和接收資料。
Shutdown / Close：結束連線並釋放資源。
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
    internal class OneTimeSocketClient
    {
        public void Run()
        {
            // 伺服器 IP 和 Port
            string serverIp = "127.0.0.1";
            int serverPort = 11000;

            try
            {
                // 建立 TCP/IP Socket
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // 連接到伺服器
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
                Console.WriteLine("連接到伺服器成功！");

                // 傳送訊息
                string message = "Hello, Server!";
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                clientSocket.Send(messageBytes);
                Console.WriteLine("傳送訊息: " + message);

                // 接收回應
                byte[] buffer = new byte[1024];
                int receivedLength = clientSocket.Receive(buffer);
                string response = Encoding.UTF8.GetString(buffer, 0, receivedLength);
                Console.WriteLine("接收到伺服器回應: " + response);

                // 關閉連線
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("發生錯誤: " + ex.Message);
            }

            Console.WriteLine("按任意鍵結束...");
            Console.ReadKey();
        }
    }
}

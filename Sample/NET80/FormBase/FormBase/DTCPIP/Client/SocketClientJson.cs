/*
### **使用方式**
#### **上傳 `DoubleValues`**
var data = new DoubleValues { Int1 = 100, String1 = "TestData" };
await AsyncSocketClientJson.UploadDataAsync(data);
```

---
#### **接收 `DoubleValues`**
DoubleValues? receivedData = await AsyncSocketClientJson.ReceiveDataAsync();
Console.WriteLine($"接收的資料: {JsonSerializer.Serialize(receivedData)}");

 */

using FormBase.DItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Client
{
    public class SocketClientJson
    {
        public static async Task UploadDataAsync(DoubleValues data)
        {
            await SendMessageAsync("UPLOAD", JsonSerializer.Serialize(data));
        }

        public static async Task<DoubleValues?> ReceiveDataAsync()
        {
            string response = await SendMessageAsync("RECEIVE", null);
            return JsonSerializer.Deserialize<DoubleValues>(response);
        }

        private static async Task<string> SendMessageAsync(string requestType, string? payload)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
                Console.WriteLine($"已連接到伺服器");

                using (NetworkStream networkStream = new NetworkStream(clientSocket, true))
                using (SslStream sslStream = new SslStream(networkStream, false))
                using (StreamWriter writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true })
                using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                {
                    await sslStream.AuthenticateAsClientAsync("localhost");

                    await writer.WriteLineAsync(requestType);
                    if (payload != null)
                    {
                        await writer.WriteLineAsync(payload);
                    }

                    string response = await reader.ReadToEndAsync();
                    Console.WriteLine($"收到伺服器回應: {response}");
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"客戶端錯誤: {ex.Message}");
                return "ERROR";
            }
            finally
            {
                clientSocket.Close();
            }
        }

    }
}

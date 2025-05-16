/*

AsyncSslStreamServiceTemplate

主要變更：
- SSL/TLS 連線：新增 SslStream 以加密通訊，防止中間人攻擊。
- 伺服器憑證：使用 X509Certificate2 來載入 .pfx 格式的憑證檔案。
- SslStream.AuthenticateAsServerAsync：伺服器使用憑證來驗證自己，確保客戶端知道它正在與合法的伺服器通訊。
- 加密資料處理：取代原本的 Socket 讀寫操作，改用 SslStream.ReadAsync 和 SslStream.WriteAsync 來處理加密資料。


當然！為了確保你的伺服器與客戶端在 SSL/TLS 下運作順暢，你可以考慮以下幾個調整與測試方法：

### **進一步調整**
1. **改進憑證驗證機制**：
   - 目前客戶端的 `ValidateServerCertificate` 只檢查 SSL 錯誤，建議新增檢查機制，例如驗證憑證的主機名稱是否與伺服器匹配。
   - 可使用 `X509Certificate2` 類別載入可信憑證來進一步增強安全性。

2. **異常處理機制強化**：
   - 目前只簡單地 `catch` 例外，建議更細化錯誤類型，例如 `AuthenticationException`、`IOException` 等，方便追蹤與除錯。
   - 在伺服器端的 `HandleClientAsync` 方法中，應考慮在 `SslStream` 處理時加入 `try-catch`，防止異常導致連線未妥善關閉。

3. **客戶端憑證驗證（選擇性）**：
   - 如果你需要更高的安全性，可以在 `AuthenticateAsServerAsync` 中設定 `clientCertificateRequired: true`，
    並要求客戶端提供憑證進行身份驗證。

4. **異步操作最佳化**：
   - 目前的 `HandleClientAsync` 在等待 `SslStream.ReadAsync` 時可能影響伺服器效能，
    建議增加 `CancellationToken` 支援，讓伺服器可更好地管理異步任務。

---

### **測試建議**
1. **基本測試：伺服器啟動與基本連線**
   - 在本機上啟動伺服器與客戶端，確認客戶端可以正確連線並交換訊息。

2. **憑證測試：確保 SSL/TLS 正確運作**
   - 使用 `openssl s_client -connect 127.0.0.1:8080` 命令行工具來測試 SSL 連接是否成功。
   - 嘗試使用過期或不匹配的憑證，確認伺服器能正確拒絕不安全的連線。

3. **負載測試：處理多個客戶端連線**
   - 啟動多個客戶端同時連線至伺服器，確保伺服器能處理並且不會因過多連線而崩潰。

4. **異常測試：模擬斷線與錯誤處理**
   - 在客戶端連線後，手動關閉伺服器，觀察客戶端是否能正確處理斷線情況。
   - 使用不正確的憑證或嘗試連線未啟動的伺服器，以測試錯誤處理是否有效。



 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace FormBase.DTCPIP.Service
{
    public class AsyncSslStreamService1
    {
        private X509Certificate2 serverCertificate = new X509Certificate2("server-cert.pfx", "password"); // 更換為你的證書檔案與密碼

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

        public async Task HandleClientAsync(Socket clientSocket)
        {
            try
            {
                Console.WriteLine("客戶端已連接");

                using (NetworkStream networkStream = new NetworkStream(clientSocket, true))
                using (SslStream sslStream = new SslStream(networkStream, false))
                {
                    await sslStream.AuthenticateAsServerAsync(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: false);

                    byte[] buffer = new byte[1024];
                    int receivedBytes = await sslStream.ReadAsync(buffer, 0, buffer.Length);
                    string clientMessage = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    Console.WriteLine($"收到客戶端訊息: {clientMessage}");

                    string response = "Hello, Secure Client!";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    await sslStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    await sslStream.FlushAsync();
                    Console.WriteLine("已回應加密訊息給客戶端");
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
                Console.WriteLine($"處理客戶端時發生錯誤: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
            }
        }

    }
}

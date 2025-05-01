using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace FormBase.DTCPIP.Client
{
    public partial class MultiClientTesterForm : Form
    {
        private int totalSend = 0;
        private int successCount = 0;
        private int failCount = 0;
        private Stopwatch stopwatch = new Stopwatch();
        public MultiClientTesterForm()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            string serverIp = txtServerIp.Text.Trim();
            int serverPort = int.Parse(txtServerPort.Text.Trim());
            int clientCount = int.Parse(txtClientCount.Text.Trim());
            int messagePerClient = int.Parse(txtMessagesPerClient.Text.Trim());

            listBoxLog.Items.Clear();
            ResetStatistics();
            stopwatch.Restart();

            listBoxLog.Items.Add($"開始啟動 {clientCount} 個 Client，每個傳 {messagePerClient} 次...");

            var tasks = new Task[clientCount];

            for (int i = 0; i < clientCount; i++)
            {
                int clientId = i + 1;
                tasks[i] = Task.Run(async () => await StartClientAsync(serverIp, serverPort, clientId, messagePerClient));
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();
            ShowStatistics();
            listBoxLog.Items.Add("所有 Client 測試完成！");
        }
        private async Task StartClientAsync(string serverIp, int serverPort, int clientId, int messageCount)
        {
            int retry = 0;
            Socket clientSocket = null;

            while (retry < 3)
            {
                try
                {
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
                    AppendLog($"Client {clientId} 已連線");

                    break;
                }
                catch
                {
                    retry++;
                    AppendLog($"Client {clientId} 第 {retry} 次連線失敗，重試中...");
                    await Task.Delay(1000);
                }
            }

            if (clientSocket == null || !clientSocket.Connected)
            {
                AppendLog($"Client {clientId} 無法連線，放棄。");
                IncreaseFail();
                return;
            }

            try
            {
                for (int i = 0; i < messageCount; i++)
                {
                    string message = $"Client {clientId} 的訊息 {i + 1}";
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    await clientSocket.SendAsync(messageBytes, SocketFlags.None);
                    IncreaseTotal();

                    byte[] buffer = new byte[1024];
                    int receivedLength = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                    string response = Encoding.UTF8.GetString(buffer, 0, receivedLength);

                    AppendLog($"Client {clientId} 收到回應: {response}");
                    IncreaseSuccess();

                    await Task.Delay(300);
                }

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                AppendLog($"Client {clientId} 錯誤: {ex.Message}");
                IncreaseFail();
            }
        }

        private void AppendLog(string message)
        {
            if (listBoxLog.InvokeRequired)
            {
                listBoxLog.Invoke(new Action<string>(AppendLog), message);
            }
            else
            {
                listBoxLog.Items.Add(message);
                listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
            }
        }

        private void IncreaseTotal()
        {
            Invoke(new Action(() => totalSend++));
        }

        private void IncreaseSuccess()
        {
            Invoke(new Action(() => successCount++));
        }

        private void IncreaseFail()
        {
            Invoke(new Action(() => failCount++));
        }

        private void ResetStatistics()
        {
            totalSend = 0;
            successCount = 0;
            failCount = 0;
            lblStatus.Text = "統計中...";
        }

        private void ShowStatistics()
        {
            lblStatus.Text = $"總送出: {totalSend}, 成功: {successCount}, 失敗: {failCount}, 總耗時: {stopwatch.Elapsed.TotalSeconds:F2} 秒";
        }
    }
}

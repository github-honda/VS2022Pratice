using Microsoft.VisualBasic.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DSample
{
    internal class TaskGetSomething
    {
        public bool Send(byte[] PBuffer, Stream PStream, CancellationToken PToken) 
            => SendTask(PBuffer, PStream, PToken).Result;
        public Task<bool> SendTask(byte[] PBuffer, Stream PStream, CancellationToken PToken) 
            => SendAsync(PBuffer, PStream, PToken);
        public async Task<bool> SendAsync(byte[] PBuffer, Stream PStream, CancellationToken PToken)
        {
            try
            {
                if (PToken.IsCancellationRequested)
                    return false;
                await PStream.WriteAsync(PBuffer, 0, PBuffer.Length, PToken);
                await PStream.FlushAsync(PToken);
                return true;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO 錯誤: {ex.Message}");
            }
            return false;
        }

        public string GetSomething() => DateTime.Now.ToString();
        public async Task<string> GetSomethingAsync() => await Task.Factory.StartNew(GetSomething);

        public string Call_ByTaskResult() => Task.Factory.StartNew(GetSomething).Result;
        public string Call_ByCodeBlock()
        {
            string sReturn = string.Empty;
            try
            {
                Task t1 = Task.Factory.StartNew(async () =>
                {
                    sReturn = await Task.Factory.StartNew(GetSomething);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return sReturn;
        }
    }
}

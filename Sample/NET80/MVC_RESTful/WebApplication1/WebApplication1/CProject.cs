/*
 
瀏覽器測試網址
https://localhost:7122/CBoxUI/Index
https://localhost:7122/CBoxUI/Create
https://localhost:7122/CBoxUI/Edit/{id}
https://localhost:7122/CBoxUI/Delete/{id}

https://localhost:7122/CPackUI/Index)
https://localhost:7122/CPackUI/Create)
https://localhost:7122/CPackUI/Edit/{id}
https://localhost:7122/CPackUI/Delete/{id}


API 測試方法, powershell
Invoke-WebRequest -Uri "https://localhost:7122/api/CBox" -Method GET
Invoke-RestMethod -Uri "https://localhost:7122/api/CBox" -Method POST -Headers @{ "Content-Type"="application/json" } -Body '{ "_Key": 1, "_Name": "Sample", "_Level": 3, "_CreateTime": "2025-05-18T19:30:00" }'
Invoke-RestMethod -Uri "https://localhost:7122/api/CBox/1" -Method PUT -Headers @{ "Content-Type"="application/json" } -Body '{ "_Key": 1, "_Name": "Updated", "_Level": 5, "_CreateTime": "2025-05-18T19:30:00" }'
Invoke-RestMethod -Uri "https://localhost:7122/api/CBox/1" -Method DELETE


Invoke-WebRequest -Uri "https://localhost:7122/api/CPack" -Method GET
Invoke-RestMethod -Uri "https://localhost:7122/api/CPack" -Method POST -Headers @{ "Content-Type"="application/json" } -Body '{ "_SeqNo": 1, "_Msg": "Hello", "_Code": 42, "_UpdateTime": "2025-05-18T20:00:00" }'
Invoke-RestMethod -Uri "https://localhost:7122/api/CPack/1" -Method PUT -Headers @{ "Content-Type"="application/json" } -Body '{ "_SeqNo": 1, "_Msg": "Updated Message", "_Code": 99, "_UpdateTime": "2025-05-18T20:30:00" }'
Invoke-RestMethod -Uri "https://localhost:7122/api/CPack/1" -Method DELETE
  
 
 */

using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace WebApplication1
{
    public class CProject : IDisposable
    {
        public CProject(string[] PArgs)
        {
            _args = PArgs;
        }
        public const string CProjectID = "SampleWebMvc";
        public const string COptionFile = $"{CProjectID}.json";
        //public readonly string _DebugFile = $"{CProjectID}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
        public const string CDebugFile = $"{CProjectID}_Debug.txt";
        public string _Name { get; private set; } = string.Empty;

        string[] _args;
        TextWriterTraceListener? _TextWriterTraceListener = null;

        public static CProject? _Me = null!;
        public COption _Option = new();



#if ZDEBUG
        public const Boolean CZDebug = true;
#else
        public const Boolean CZDebug = false;
#endif
#if ZTRACE
        public const Boolean CZTrace = true;
#else
        public const Boolean CZTrace = false;
#endif
#if ZTEST
        public const Boolean CZTest = true;
#else
        public const Boolean CZTest = false;
#endif
        public void Run()
        {
            try
            {
                Debug.WriteLine("Run()");
                _Option.SetProjectDefault();

                // 設定檔
                if (!LoadOption())
                {
                    string sDefaultFile = $"{CProjectID}.default.json";
                    SaveOption(sDefaultFile);
                    throw new Exception($"無法讀取啟動設定檔={COptionFile}. 請連繫原廠取得, 或參考 {sDefaultFile} 提供正確的設定值.");
                }
                if (_Option.ID != CProjectID)
                    throw new Exception($"設定檔 ID 錯誤.");

                _Name = _Option.Name;
                Debug.WriteLine($"ProjectName={_Name}.");

                EnableDebug(_Option.Debug == 1);
                Debug.WriteLine($"ConnectionString={_Option!.ConnectionString}.");

                // 主程式

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            Console.Write("End run, Type any key to continue.");
            Console.Read();
        }

        public void EnableDebug(bool PEnable)
        {
            if (PEnable)
            {
                Trace.Listeners.Clear();
                _TextWriterTraceListener = new(File.CreateText(CDebugFile));
                Trace.Listeners.Add(_TextWriterTraceListener);
                Trace.AutoFlush = true;
            }
        }

        public bool LoadOption()
        {
            try
            {
                using FileStream PStream_utf8Json = File.OpenRead(COptionFile);
                COption? option1 = JsonSerializer.Deserialize<COption>(PStream_utf8Json);
                if (option1 == null)
                    throw new Exception($"LoadOptions() failed. File = {COptionFile}");

                // 檢查設定檔
                if (option1.ID != CProjectID)
                    throw new Exception($"設定值錯誤. option1.ID= {option1.ID}. ID mismatch.");

                _Option = option1;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        public void SaveOption(string PFile)
        {
            using FileStream stream1 = File.Create(PFile);
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(_Option, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs), });
            stream1.Write(bytes, 0, bytes.Length);
        }
        #region IDisposable pattern
        private bool _Disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    _TextWriterTraceListener?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _Disposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CProject()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

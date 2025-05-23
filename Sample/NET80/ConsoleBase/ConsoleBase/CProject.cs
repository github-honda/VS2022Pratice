﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using ConsoleBase.DJson;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Xml.Linq;

namespace ConsoleBase
{
    public class CProject : IDisposable
    {
        public CProject(string[] PArgs)
        {
            _args = PArgs;
        }
        public const string CProjectID = "SampleConsole";
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
                //var test1 = new FlatJsonConvert_Test();
                //test1.Run();

                var test2 = new TreeString4_Test();
                test2.Run();
                Console.WriteLine($"DebugFile={CDebugFile}");

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

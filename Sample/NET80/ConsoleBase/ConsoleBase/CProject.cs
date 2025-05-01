using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace ConsoleBase
{
    internal class CProject: IDisposable
    {

        public CProject(string[] PArgs)
        {
            _args = PArgs;
            Trace.Listeners.Clear();
            _Log = new(File.CreateText($"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}"));
            Trace.Listeners.Add(_Log);
            Trace.AutoFlush = true; 
        }
        public const string CProjectName = "CProject";
        string[] _args;
        readonly TextWriterTraceListener _Log; 
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
            Debug.WriteLine("Run()");
        }
        public void Function1()    
        {
            Debug.WriteLine("Function1()");
        }

        #region IDisposable pattern
        private bool _Disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    _Log.Dispose();
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

/*
 
https://stackoverflow.com/questions/17562295/if-i-allocate-some-memory-with-allochglobal-do-i-have-to-free-it-with-freehglob
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DMarshal
{
    public class StructWrapper : IDisposable
    {

        public StructWrapper(object? obj)
        {
            //if (_IntPtr != null)
            //{
            //    _IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
            //    Marshal.StructureToPtr(obj, _IntPtr, false);
            //}
            //else
            //{
            //    _IntPtr = IntPtr.Zero;
            //}

            if (obj == null)
                _IntPtr = IntPtr.Zero;
            else
            {
                _IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
                Marshal.StructureToPtr(obj, _IntPtr, false);
            }
        }

        public IntPtr _IntPtr { get; private set; }

        public static implicit operator IntPtr(StructWrapper PStructWrapper) => PStructWrapper._IntPtr;


        #region Dispose pattern
        private bool _Disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                if (_IntPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_IntPtr);
                    _IntPtr = IntPtr.Zero;
                }
                //GC.SuppressFinalize(this); // 放到 destructor 中

                _Disposed = true;
            }
        }

        // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~StructWrapper()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}

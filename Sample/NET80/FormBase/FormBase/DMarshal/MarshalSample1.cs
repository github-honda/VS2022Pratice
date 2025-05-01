/*
 
https://alomardev.wordpress.com/2017/04/07/c-convert-object-to-byte-array-byte-array-to-object/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DMarshal
{
    public class MarshalSample1
    {
        public void ObjectToBytes()
        {
            MarshalSample2 class1 = new();
            int size = Marshal.SizeOf(class1);
            byte[] bytes = new byte[size];

            nint ptr = Marshal.AllocHGlobal(size);

            // Copy object byte-to-byte to unmanaged memory.
            Marshal.StructureToPtr(class1, ptr, false);

            // Copy data from unmanaged memory to managed buffer.
            Marshal.Copy(ptr, bytes, 0, size);

            // Release unmanaged memory.
            Marshal.FreeHGlobal(ptr);
        }

        public void BytesToObject()
        {
            MarshalSample2? class1 = new();
            int size = Marshal.SizeOf(class1);
            byte[] bytes = new byte[size];

            nint ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, ptr, size);
            //p = (ZMarshalSample2)Marshal.PtrToStructure(ptr, typeof(ZMarshalSample2));
            class1 = Marshal.PtrToStructure(ptr, typeof(MarshalSample2)) as MarshalSample2;
            Marshal.FreeHGlobal(ptr);
        }

    }
}

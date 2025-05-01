/*
 
https://alomardev.wordpress.com/2017/04/07/c-convert-object-to-byte-array-byte-array-to-object/
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DMarshal
{
    public class MarshalSample3
    {
        public void ObjectToBytes()
        {
            MarshalSample2 class1 = new();
            class1.age = 1;
            class1.name = "Test";

            /*
System.ArgumentException
  HResult=0x80070057
  Message=Type 'FormBase.DMarshal.MarshalSample2' cannot be marshaled as an unmanaged structure; no meaningful size or offset can be computed.
  Source=System.Private.CoreLib
  StackTrace:
   at System.Runtime.InteropServices.Marshal.SizeOfHelper(Type t, Boolean throwIfNotMarshalable)
   at System.Runtime.InteropServices.Marshal.SizeOf[T](T structure) in /_/src/libraries/System.Private.CoreLib/src/System/Runtime/InteropServices/Marshal.cs:line 110
   at FormBase.DMarshal.MarshalSample3.ObjectToBytes() in U:\RD\src\Z2025\Sample\FormBase\FormBase\DMarshal\MarshalSample3.cs:line 19
   at FormBase.Form1.MyMarshalTest() in U:\RD\src\Z2025\Sample\FormBase\FormBase\Form1.cs:line 44
   at FormBase.Form1.btnMarshal_Click(Object sender, EventArgs e) in U:\RD\src\Z2025\Sample\FormBase\FormBase\Form1.cs:line 32
   at System.Windows.Forms.Button.OnClick(EventArgs e) in System.Windows.Forms\Button.cs:line 240
   at System.Windows.Forms.Button.OnMouseUp(MouseEventArgs mevent) in System.Windows.Forms\Button.cs:line 268
   at System.Windows.Forms.Control.WmMouseUp(Message& m, MouseButtons button, Int32 clicks) in System.Windows.Forms\Control.cs:line 13993
   at System.Windows.Forms.Control.WndProc(Message& m) in System.Windows.Forms\Control.cs:line 14508
   at System.Windows.Forms.ButtonBase.WndProc(Message& m) in System.Windows.Forms\ButtonBase.cs:line 1338
   at System.Windows.Forms.Control.ControlNativeWindow.WndProc(Message& m) in System.Windows.Forms\Control.cs:line 2863
   at System.Windows.Forms.NativeWindow.Callback(IntPtr hWnd, WM msg, IntPtr wparam, IntPtr lparam) in System.Windows.Forms\NativeWindow.cs:line 351
   at Interop.User32.DispatchMessageW(MSG& msg)
   at System.Windows.Forms.Application.ComponentManager.Interop.Mso.IMsoComponentManager.FPushMessageLoop(UIntPtr dwComponentID, msoloop uReason, Void* pvLoopData) in System.Windows.Forms\Application.cs:line 236
   at System.Windows.Forms.Application.ThreadContext.RunMessageLoopInner(msoloop reason, ApplicationContext context) in System.Windows.Forms\Application.cs:line 1318
   at System.Windows.Forms.Application.ThreadContext.RunMessageLoop(msoloop reason, ApplicationContext context) in System.Windows.Forms\Application.cs:line 1236
   at FormBase.Program.Main() in U:\RD\src\Z2025\Sample\FormBase\FormBase\Program.cs:line 14
             */
            int size = Marshal.SizeOf(class1); // UTF8 中文字串轉換, 在這裡就錯誤了

            byte[] bytes = new byte[size];
            using (StructWrapper wrapper = new StructWrapper(class1))
            {
                Marshal.StructureToPtr(class1, wrapper._IntPtr, false);

                // Copy data from unmanaged memory to managed buffer.
                Marshal.Copy(wrapper._IntPtr, bytes, 0, size);
            }
        }

        public void BytesToObject()
        {
            MarshalSample2? class1 = new();
            int size = Marshal.SizeOf(class1);
            byte[] bytes = new byte[size];
            using (StructWrapper wrapper = new StructWrapper(class1))
            {
                Marshal.Copy(bytes, 0, wrapper._IntPtr, size);
                class1 = Marshal.PtrToStructure(wrapper._IntPtr, typeof(MarshalSample2)) as MarshalSample2;
            }
        }

        public byte[] ObjectToBytes(MarshalSample2 PClass)
        {
            //MarshalSample2 class1 = new();
            int size = Marshal.SizeOf(PClass);
            byte[] bytes = new byte[size];
            using (StructWrapper wrapper = new StructWrapper(PClass))
            {
                Marshal.StructureToPtr(PClass, wrapper._IntPtr, false);

                // Copy data from unmanaged memory to managed buffer.
                Marshal.Copy(wrapper._IntPtr, bytes, 0, size);
            }
            return bytes;
        }
        public MarshalSample2? BytesToObject(byte[] PBytes)
        {
            MarshalSample2? class1 = new();
            int size = Marshal.SizeOf(class1);
            if (size != PBytes.Length)
                throw new ArgumentException($"PBytes.Length != {size}.");

            byte[] bytes = new byte[size];
            using (StructWrapper wrapper = new StructWrapper(class1))
            {
                Marshal.Copy(bytes, 0, wrapper._IntPtr, size);
                class1 = Marshal.PtrToStructure(wrapper._IntPtr, typeof(MarshalSample2)) as MarshalSample2;
            }
            return class1;
        }

        public bool TryObjectToBytes(MarshalSample2 PClass, [NotNullWhen(true)] out byte[]? PBytes, [NotNullWhen(false)] out Exception? PException)
        {
            PException = null;
            PBytes = null;
            try
            {
                int size = Marshal.SizeOf(PClass);
                PBytes = new byte[size];
                using (StructWrapper wrapper = new StructWrapper(PClass))
                {
                    Marshal.StructureToPtr(PClass, wrapper._IntPtr, false);

                    // Copy data from unmanaged memory to managed buffer.
                    Marshal.Copy(wrapper._IntPtr, PBytes, 0, size);
                }
                return true;
            }
            catch (Exception ex1) 
            {
                PException = ex1;
                return false;
            }
        }
        public bool TryBytesToObject(byte[] PBytes, [NotNullWhen(true)] out MarshalSample2? PClass, [NotNullWhen(false)] out Exception? PException)
        {
            PException = null;
            PClass = null;
            try
            {
                PClass = new();
                int size = Marshal.SizeOf(PClass);
                if (size != PBytes.Length)
                    throw new ArgumentException($"PBytes.Length != {size}.");

                byte[] bytes = new byte[size];
                using (StructWrapper wrapper = new StructWrapper(PClass))
                {
                    Marshal.Copy(bytes, 0, wrapper._IntPtr, size);
                    PClass = Marshal.PtrToStructure(wrapper._IntPtr, typeof(MarshalSample2)) as MarshalSample2;
                }
                if (PClass == null)
                    throw new Exception("Marshal.PtrToStructure() return null.");
                return true;
            }
            catch (Exception ex1)
            {
                PException = ex1;
                return false;
            }

        }


    }
}

/* 
Remarks:
The IAsyncResult interface is implemented by classes containing methods that can operate asynchronously. 
物件中的方法, 可透過 IAsyncResult 介面實作為非同步呼叫.

It is the return type of methods that initiate an asynchronous operation, such as FileStream.BeginRead, 
and it is passed to methods that conclude an asynchronous operation, such as FileStream.EndRead. 

IAsyncResult objects are also passed to methods invoked by AsyncCallback delegates when an asynchronous operation completes.
An object that supports the IAsyncResult interface stores state information for an asynchronous operation and provides a synchronization object to allow threads to be signaled when the operation completes.

Note
The AsyncResult class is the implementation of IAsyncResult that is returned by the BeginInvoke method when you use a delegate to call a method asynchronously.
 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DSample
{
    public class IAsyncResultSample // CodeHelper IAsyncResult
    {
        // The method to be executed asynchronously.
        public string TestMethod(int callDuration, out int threadId)
        {
            Console.WriteLine("Test method begins.");
            Thread.Sleep(callDuration);
            //ZThreading.ZSleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;
            return String.Format("My call time was {0}.", callDuration.ToString());
        }
        // The delegate must have the same signature as the method it will call asynchronously.
        public delegate string AsyncMethodCaller(int callDuration, out int threadId); // CodeHelper IAsyncResult, CodeHelper delegate. 將內部方法改為可非同步呼叫.
    }
}


﻿/* This example produces output similar to the following:

Main thread 1 does some work.
Test method begins.
The call executed on thread 3, with return value "My call time was 3000.".

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormBase.DSample.IAsyncResultSample;
//using FormBase.DSample.IAsyncResultSample;

namespace FormBase.DSample
{
    public class IAsyncResultMainSample
    {
        void Main_IAsyncResult()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            IAsyncResultSample ad = new();

            // Create the delegate.
            AsyncMethodCaller caller = new(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000, out threadId, null, null);

            Thread.Sleep(0);
            Console.WriteLine("Main thread {0} does some work.", Thread.CurrentThread.ManagedThreadId);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            // Perform additional processing here.
            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".", threadId, returnValue);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DSample
{
    internal class ManualResetEventSample
    {
        /*
        Initializes a new instance of the ManualResetEvent class with a Boolean value indicating whether to set the initial state to signaled.
        public readonly static ManualResetEvent MDoneEvent = new(true);  // CodeHelper ManualResetEvent. 建立 Signaled 的 Event,    不會停止目前執行緒,   直到 Reset() 開始停止執行緒.
        public readonly static ManualResetEvent MDoneEvent = new(false); // CodeHelper ManualResetEvent. 建立 Nonsignaled 的 Event, 立即停止目前的執行緒. 直到 Set() 執行.          
        public ManualResetEvent (bool initialState);
          initialState:
              true to set the initial state signaled; 
              false to set the initial state to nonsignaled.
          True:  If the initial state of a ManualResetEvent is signaled (that is, if it is created by passing true for initialState), threads that wait on the ManualResetEvent do not block. 
          false:  If the initial state is nonsignaled, threads block until the Set method is called.
          https://learn.microsoft.com/en-us/dotnet/api/system.threading.manualresetevent.-ctor?view=net-8.0&devlangs=csharp&f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Threading.ManualResetEvent.%2523ctor)%3Bk(DevLang-csharp)%26rd%3Dtrue
        


         */

        /// <summary>
        /// 共用訊號範例.
        /// Trace 這個變數可取得 ManualResetEvent 完整範例用法.
        /// 可控制目前的執行緒停止執行或繼續執行.
        /// 若須停止執行, 則呼叫 WaitOne(TimeSpan).
        /// 若需繼續執行, 則呼叫 Set().
        /// </summary>
        public readonly ManualResetEvent MDoneEvent = new(false);
        /// <summary>
        /// 停止執行, 等到接收到訊號後, 再繼續執行.
        /// 若 Timeout &lt;= 0, 則無限時間等待.
        /// </summary>
        /// <returns></returns>
        public bool ZEventWaitOne(TimeSpan Timeout)
        {
            // Blocks the current thread until the current instance receives a signal, using a TimeSpan to specify the time interval.
            return ZWaitOne(MDoneEvent, Timeout);  // CodeHelper ManualResetEvent.WaitOne(); 停止執行緒直到 Timeout.
        }


        /// <summary>
        /// 發出訊號可繼續執行.
        /// 若沒有行程正在等待訊號, 則呼叫本函數無影響.
        /// </summary>
        /// <returns></returns>
        public bool ZEventSet()
        {
            // Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
            // Also, if Set is called when there are no threads waiting and the EventWaitHandle is already signaled, the call has no effect.
            return MDoneEvent.Set(); // CodeHelper ManualResetEvent.Set(); 繼續執行執行緒: 將 Event signaled. 
        }

        /// <summary>
        /// 重設為無訊號狀態.
        /// 建議自行宣告 ManualResetEvent 使用相同的功能.
        /// </summary>
        /// <returns></returns>
        public bool ZEventReset()
        {
            // Sets the state of the event to nonsignaled, causing threads to block.
            return MDoneEvent.Reset(); // CodeHelper ManualResetEvent.Reset(); 停止執行執行緒, 將 Event nonsignaled.
        }
        /// <summary>
        /// 停止目前的執行緒, 直到 PTimeout 或 signaled, 再繼續執行.
        /// 停止執行, 等到接收到訊號後, 再繼續執行.
        /// 若 Timeout &lt;= 0, 則無限時間等待.
        /// </summary>
        /// <param name="PEvent"></param>
        /// <param name="PTimeout"></param>
        public  bool ZWaitOne(ManualResetEvent PEvent, TimeSpan PTimeout)
        {
            // Blocks the current thread until the current instance receives a signal, using a TimeSpan to specify the time interval.
            if (PTimeout > TimeSpan.Zero)
                return PEvent.WaitOne(PTimeout);
            else
                return PEvent.WaitOne();
        }

    }
}

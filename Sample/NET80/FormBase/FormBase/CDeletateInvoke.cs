using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FormBase
{


    public static class CDeletateInvoke
    {
        // 代理函數定義型別 
        delegate string DelegateCaller(int PInt, out string PString);

        //  move to sample project
        //static string? SampleTextBoxText;
        //static string? SampleTextBoxText2;


        static void MyBeginInvokeCallback(IAsyncResult? PAsyncResult)
        {
            // 新執行緒回呼本函數.

            // IAsyncResult.AsyncState 可取得傳入的 StateObject.
            //string? UserObject = PAsyncResult?.AsyncState?.ZToString_Nullable();

            // 通常是改變一個共用的元素, 例如Winform 畫面上一個共用的 control, 而且不需要回傳值.
            // todo: move to Sample project.
            //SampleTextBoxText.tex = DateTime.Now.ZToString();
            //SampleTextBoxText2 = UserObject;

            // 如果需要 BeginInvoke() 回傳值, 則建議應在呼叫 BeginInvoke() 的函數中, 呼叫 EndInvoke() 取得執行結果.
        }


    }
}

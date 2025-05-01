using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DMarshal
{
    [Serializable]
    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class MarshalSample2
    {
        public int age;

        /// <summary>
        /// 若為 string 型別, 則必須加入 [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] 才能正確計算大小
        /// </summary>
        // 有使用到 string 類型, 必須加入這段讓 Marshal 才能正確計算大小
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBase.DItem
{
    /// <summary>
    /// TreeString4.
    /// 巢狀的 String4, 適合存放小量的 String 資料, 作為交換資料用途.
    /// </summary>
    public class TreeString4: String4
    {
        public TreeString4[]? Items { get; set; } = null;
    }
}

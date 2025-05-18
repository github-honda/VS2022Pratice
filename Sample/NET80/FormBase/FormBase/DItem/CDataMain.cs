using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DItem
{
    public class CDataMain
    {
        public long _No { get; set; } = 0;

        public string? _Name { get; set; } = null;
        public int _Qty { get; set; } = 0;
        public DateTime CreateTime { get; set; } = DateTime.MinValue;
        public byte[]? Bytes1 { get; set; } = null;


    }
}

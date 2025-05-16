using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DItem
{
    /// <summary>
    /// 萬用的 Values.
    /// 存放 Value 型別, string, byte[] 型別各 2 個.
    /// </summary>
    public class DoubleValues
    {
        public int Int1 { get; set; } = 0;
        public int Int2 { get; set; } = 0;
        public long Long1 { get; set; } = 0;
        public long Long2 { get; set; } = 0;
        public double Double1 { get; set; } = 0d;
        public double Double2 { get; set; } = 0d;
        public decimal Decimal1 { get; set; } = 0m;
        public decimal Decimal2 { get; set; } = 0m;
        public DateTime DateTime1 { get; set; } = DateTime.MinValue;
        public DateTime DateTime2 { get; set; } = DateTime.MinValue;
        public string? String1 { get; set; } = null;
        public string? String2 { get; set; } = null;
        public byte[]? Bytes1 { get; set; } = null;
        public byte[]? Bytes2 { get; set; } = null;
    }
}

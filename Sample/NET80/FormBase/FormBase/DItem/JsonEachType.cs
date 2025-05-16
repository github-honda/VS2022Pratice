using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DItem
{
    /// <summary>
    /// 測試 JsonSerialize, Deserialize 資料格式.
    /// </summary>
    public class JsonEachType
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
        public string? StringNull { get; set; } = null;
        public string StringEmpty { get; set; } = string.Empty;
        public string String1 { get; set; } = string.Empty;
        public byte[]? BytesNull { get; set; } = null;
        public byte[]? Bytes0 { get; set; } = [];
        public byte[]? Bytes1 { get; set; } = null;


        public int[]? IntArrayNull { get; set; } = null;
        public int[] IntArray0 { get; set; } = [];
        public int[] IntArray1 { get; set; } = new int[] { 1, 2, 3, 4, 5 };

        public string[]? StringArrayNull { get; set; } = null;
        public string[] StringArray0 { get; set; } = [];
        public string[] StringArray1 { get; set; } = new string[] { "第1個", "第2個", "第3個", "第4個", "第5個" };

        public DoubleValues? DoubleValuesNull { get; set; } = null;
        public DoubleValues DoubleValue1 { get; set; } = new DoubleValues();

        public void SerializeTest(string PFile)
        {
            Int1 = 1;
            Int2 = 2;
            Long1 = 1;
            Long2 = 2;
            Double1 = 1.23456d;
            Double2 = 2.34567d;
            Decimal1 = 3.45678m;
            Decimal2 = 4.56789m;
            DateTime1 = DateTime.Now;
            DateTime2 = DateTime.UtcNow;
            String1 = "1234中文測試Json存放資料的格式";
            Bytes1 = Encoding.UTF8.GetBytes(String1);
            CCommon.SerializeToFile(this, PFile);
            DoubleValue1 = new DoubleValues();
        }


    }
}

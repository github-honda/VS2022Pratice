using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBase.DItem
{
    /// <summary>
    /// String4.
    /// 固定存放 4個字串, 分別為 Name, Text, Value, Type.
    /// Value 和 Type 皆為 Nullable, 其他兩個不為 Nullable.
    /// </summary>
    public class String4
    {
        public string Name { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string? Value { get; set; } = null;
        public string? Type { get; set; } = null;
    }
}

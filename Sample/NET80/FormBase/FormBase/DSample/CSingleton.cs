/*
Singleton Pattern
  
Singleton 模型 應只有1個 唯一的 instance..

  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DSample
{
    // 禁止被繼承.
    public sealed class CSingleton // CodeHelper Singleton.
    {
        // 禁止被實例化.
        private CSingleton() { }

        // 唯一的 instance.
        public static CSingleton Instance { get; } = new CSingleton(); 
    }

    public static class CSingletonShared // CodeHelper Singleton shared.
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);
        static readonly object _Lock = new();

        public static int Next()
        {
            lock (_Lock)
            {
                return _random.Next();
            }
        }

        public static int Next(int minValue, int maxValue)
        {
            lock (_Lock)
            {
                return _random.Next(minValue, maxValue);
            }
        }
    }
}

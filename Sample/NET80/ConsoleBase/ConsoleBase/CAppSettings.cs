using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleBase
{
    public class CAppSettings
    {
        // 欄位可以少於 .json 內容.
        public TimeSpan AutoRetryDelay { get; set; }
        public LogLevel MinimumLogLevel { get; set; }
        public string? LogTimeFormat { get; set; }
        public string? ConnectionString { get; set; }
        public string? NotFind1 { get; set; }
        public bool Debug { get; set; }
        public bool Trace { get; set; }
        public bool Test { get; set; }

        //public void BindJson(ConfigurationManager PCm, string PSection)
        //{
        //    // 原寫法
        //    //CSecret Secret1 = new();
        //    //_CM.GetSection(nameof(CSecret)).Bind(Secret1);
        //    //T? aa = (T?)ZType.ZCreateInstance(typeof(T));
        //    //T r1 = ZType.ZCreateInstance<T>();
        //    PCm.GetSection(PSection).Bind(this);
        //    //return r1;
        //}
    }

}

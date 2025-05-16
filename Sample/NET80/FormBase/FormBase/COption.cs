using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase
{
    /// <summary>
    /// 專案設定檔.
    /// 注意: 屬性名稱必須對應.json設定檔的名稱, 否則無法載入屬性值.
    /// </summary>
    public class COption
    {
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public int Debug { get; set; } = 0;
        public int Log { get; set; } = 0;

        public void SetProjectDefault()
        {
            ID = CProject.CProjectID;
            Name = "專案名稱";
            Version = "1.0.0";
            ConnectionString = "HostDatabaseUserPassword";
            Debug = 0;
            Log = 0;
        }

    }
}

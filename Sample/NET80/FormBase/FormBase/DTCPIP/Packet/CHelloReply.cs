using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Packet
{
    public class CHelloReply
    {
        public DateTime SentTime_UTC { get; set; }

        /// <summary>
        /// 每個 Client 以 ClientKey 計算自己的 ClientSecretKey 上傳給 Service.
        /// Service 後續服務將可以 ClientSecretKey 加工傳送資料給 Client. 
        /// </summary>
        public long ClientSecret { get; set; } = 0;

        public string Nickname { get; set; } = string.Empty;
        public string LoginID { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string StationName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }
}

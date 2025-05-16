using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Packet
{
    public class SHello
    {
        public DateTime SentTime_UTC { get; set; }

        /// <summary>
        /// Service 授予每個連線一個暫時的用戶編號.
        /// </summary>
        public long ClientNo { get; set; } = 0;


        // 每個暫時用戶編號使用不同的 Key 存取服務
        public long ClientKey { get; set; } = 0;


        public string ServerName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }
}

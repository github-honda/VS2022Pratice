using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Packet
{
    public class SHelloConfirm
    {
        public DateTime SentTime_UTC { get; set; }

        /// <summary>
        /// 使用 Service 服務的 Key.
        /// </summary>
        public long AccessKey { get; set; } = 0;
    }
}

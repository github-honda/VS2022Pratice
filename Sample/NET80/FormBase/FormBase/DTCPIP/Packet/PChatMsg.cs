using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Packet
{
    public class PChatMsg
    {
        public DateTime SentTime_UTC { get; set; }

        public long To_ClientNo { get; set; } = 0;
        public long From_ClientNo { get; set; } = 0;

        public string Msg { get; set; } = string.Empty;
    }
}

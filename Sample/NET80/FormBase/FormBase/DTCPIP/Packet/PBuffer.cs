using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Packet
{
    public class PBuffer
    {
        public DateTime SentTime_UTC { get; set; }

        public long To_ClientNo { get; set; } = 0;
        public long From_ClientNo { get; set; } = 0;

        public long HandleNo { get; set; } = 0;
        public long SeqNo { get; set; } = 0;
        public bool IsEnd { get; set; } = false;

        public byte[] BufferData { get; set; } = [];

    }
}

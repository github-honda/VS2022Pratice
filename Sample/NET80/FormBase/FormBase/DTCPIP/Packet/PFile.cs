using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Packet
{
    public class PFile
    {
        public DateTime SentTime_UTC { get; set; }


        public long To_ClientNo { get; set; } = 0;
        public long From_ClientNo { get; set; } = 0;

        public string Filename { get; set; } = string.Empty;
        public string Folder { get; set; } = string.Empty;
        public long FileSize { get; set; } = 0;
        public byte FileChecksum { get; set; } = 0;

        public long BufferHandleNo { get; set; } = 0;

    }
}

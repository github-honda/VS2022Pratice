using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP.Packet
{
    public class PCommand
    {
        public DateTime SentTime_UTC { get; set; }

        /// <summary>
        /// 1=Command, 
        /// 900=ResponseOK, 
        /// 905=ResponseError, 
        /// 950=EndConnect, 
        /// 955=EndService.
        /// </summary>
        public int CmdCode { get; set; } = 0;
        public string? Message { get; set; } = null;

        public string? Cmd { get; set; } = null;
        public string? CmdType { get; set; } = null;
        public string? CmdArg { get; set; } = null;

        public long RefSubjecNo { get; set; } = 0;
        public long RefObjectNo { get; set; } = 0;

    }
}

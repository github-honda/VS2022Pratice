using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using ZLib;
using ZLib.DIO;
using ZLib.DLog;

namespace ConsoleBase
{
    #region template for CLog: 以  ZZLog.cs 為準. (或 Template ZLibFormBase.CLog.cs, 或 Template ZLibConsoleBase.CLog.cs).
    /// <summary>
    /// Template of CLog.cs.
    /// </summary>
    internal sealed class CLog : ZZLog
    {
        static CLog()
        {
            try
            {
                // 存放 log 檔案資料夾應該在程式執行前已建立, 並且有權限可使用.
                // 例如, 系統安裝過程中就應該要確認資料夾可存放 log 檔案.
                //string LogPath = ZIO.ZCombine(ZCommon.ZGetLaunchPath(), "App_Data", "Log");
                string LogPath = ZCommon.GetPathLaunch();
                //string LogPath = ZIO.ZGetTempPath();
                if (!ZIO.ZExistsDirectory(LogPath))
                    throw new DirectoryNotFoundException(LogPath);

                Me = new CLog(CProject.CProjectID, LogPath);
            }
            catch
            {
                Me = null;
            }
        }
        public CLog(string PSubject, string PFolder)
                : base(PSubject, PFolder)
        {
            MTraceEnabled = true;
            MDebugEnabled = true;
            //MColumn_DateTimeFormat = ZDateTime.CFormatMsDelimited;
            //_ColumnTimeFormat = string.Empty;
            MColumn_DateTimeFormat = string.Empty;
            //MColumn_LogLevel = true;
            //MColumn_ThreadID = true;
            MColumn_LogLevel = false;
            MColumn_ThreadID = true;
            MDelayMs = 3000;
            MMaxQueues = 80000;
        }
        public static CLog? Me { get; private set; } = null;
    }
    #endregion
}

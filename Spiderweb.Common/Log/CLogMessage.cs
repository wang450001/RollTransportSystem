using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.log
{
    /* *
     * 
     * 功能：日志信息类
     * 
     * 作者：Spiderweb
     * 
     * 日期：2020-8-2
     * 
     * */
    public class CLogMessage
    {
        public MessageType LogLevel { get; set; }

        /// <summary>
        /// 日志的关键码
        /// </summary>
        public string LogKeyCode { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 日志产生时间
        /// </summary>
        public DateTime ActionTime { get; set; }

        public CLogMessage(MessageType level, string content, string keyCode)
        {
            LogLevel = level;
            Content = content;
            LogKeyCode = keyCode;
            ActionTime = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("[{0}] [{1}] {2} {3}", LogLevel, Spiderweb.Utils.CommonUtils.FormatDate(ActionTime), Content, LogKeyCode);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.log
{
    public enum MessageType
    {
        Debug,Info,Warn,Error,Fatal
    }

     public interface ISpiderwebLog
    {
        void Log(MessageType type, object message);
    }
}

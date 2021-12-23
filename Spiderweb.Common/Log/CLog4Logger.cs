using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.log
{
    public class CLog4Logger : CLogger
    {
        ILog logger = null;

        public CLog4Logger() : this(null, null) { }

        public CLog4Logger(string path) : this(path, null) { }

        public CLog4Logger(string path, string name)
            : base(path, name)
        {
            if (string.IsNullOrEmpty(LoggerPath)) return;

            FileInfo configFile = new FileInfo(LoggerPath);

            log4net.Config.XmlConfigurator.ConfigureAndWatch(configFile);
        }

        public override void Log(MessageType type, object message)
        {
            if (string.IsNullOrEmpty(LoggerName)) return;

            logger = LogManager.GetLogger(LoggerName);
            if (logger == null) return;

            switch (type)
            {
                case MessageType.Debug:
                    logger.Debug(message);
                    break;
                case MessageType.Warn:
                    logger.Warn(message);
                    break;
                case MessageType.Error:
                    logger.Error(message);
                    break;
                case MessageType.Fatal:
                    logger.Fatal(message);
                    break;
                default:
                    logger.Info(message);
                    break;
            }

            logger = null;
        }
    }
}

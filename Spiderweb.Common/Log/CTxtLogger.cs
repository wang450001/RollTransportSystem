using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spiderweb.Common.log
{
    public class CTxtLogger : CLogger
    {
        TextWriter tw;
        Semaphore writeSignal;
        ConcurrentQueue<string> msgQueue;
        System.Threading.Timer writeMsgTimer;
        Semaphore synSignal;

        public CTxtLogger() : this(null, null) { }

        public CTxtLogger(string path) : this(path, null) { }

        public override string LoggerName
        {
            get
            {
                return Spiderweb.Utils.CommonUtils.FormatDate(23);
            }
            set
            {
                base.LoggerName = value;
            }
        }

        public CTxtLogger(string path, string name)
            : base(path, name)
        {
            tw = null;
            writeSignal = new Semaphore(1, 1);
            msgQueue = new ConcurrentQueue<string>();
            writeMsgTimer = new Timer((object data) =>
            {
                Write();
            }, null, 1000, 1000);
            synSignal = new Semaphore(1, 1);
        }

        private void Write(string msg)
        {
            if (string.IsNullOrEmpty(LoggerPath) || string.IsNullOrEmpty(LoggerName)) return;

            string filePath = string.Format("{0}\\{1}年\\{2}月", LoggerPath, DateTime.Now.Year, DateTime.Now.Month);
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            //writeSignal.WaitOne();

            try
            {
                tw = new StreamWriter(string.Format("{0}\\{1}.log", filePath, LoggerName), true, Encoding.UTF8);
                if (tw == null) return;

                tw.WriteLine(msg);
            }
            finally
            {
                if (null != tw)
                {
                    tw.Flush();
                    tw.Close();
                }
                tw = null;
                //writeSignal.Release();
            }
        }

        private void Write()
        {
            if (string.IsNullOrEmpty(LoggerPath) || string.IsNullOrEmpty(LoggerName)) return;

            string filePath = string.Format("{0}\\{1}年\\{2}月", LoggerPath, DateTime.Now.Year, DateTime.Now.Month);
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            if (msgQueue.Count < 1) return;

            synSignal.WaitOne();
            string msg = "";
            try
            {
                tw = new StreamWriter(string.Format("{0}\\{1}.log", filePath, LoggerName), true, Encoding.UTF8);
                if (tw == null) return;

                for (int i = 0; i < msgQueue.Count; i++)
                {
                    if (!msgQueue.TryDequeue(out msg)) continue;
                    tw.WriteLine(msg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (null != tw)
                {
                    tw.Flush();
                    tw.Close();
                }
                tw = null;
                synSignal.Release();
            }
        }

        public override void Log(MessageType type, object message)
        { 
            string prefix = "";
            switch (type)
            {
                case MessageType.Debug:
                    prefix = "◎";
                    break;
                case MessageType.Warn:
                    prefix = "▲";
                    break;
                case MessageType.Error:
                    prefix = "◆";
                    break;
                case MessageType.Fatal:
                    prefix = "★";
                    break;
                default:
                    prefix = "※";
                    break;
            }

            //Write(string.Format("[{0}] {1} {2}", Spiderweb.Utils.CommonUtils.FormatDate(), prefix, message));
            msgQueue.Enqueue(string.Format("[{0}] {1} {2}", Spiderweb.Utils.CommonUtils.FormatDate(), prefix, message));
        }
    }
}

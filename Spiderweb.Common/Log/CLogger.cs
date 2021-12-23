using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.log
{
    public abstract class CLogger : ISpiderwebLog
    {
        /// <summary>
        /// 日志文件名称
        /// </summary>
        public virtual string LoggerName
        {
            get;
            set;
        }

        /// <summary>
        /// 日志保存路径
        /// </summary>
        public string LoggerPath { get; protected set; }

        protected CLogger() : this(null, null) { }

        protected CLogger(string path) : this(path, null) { }

        protected CLogger(string path, string name)
        {
            this.LoggerPath = path;
            this.LoggerName = name;
        }

        public static CLogger CreateInstance(string typeName, string path)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(path)) return null;

            return (CLogger)Activator.CreateInstance(Type.GetType(typeName), path);
        }

        public static CLogger CreateInstance(string typeName, string path, string name)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(path) || string.IsNullOrEmpty(name)) return null;

            return (CLogger)Activator.CreateInstance(Type.GetType(typeName), path, name);
        }


        public virtual void Log(MessageType type, object message)
        {
            
        }

        public void Log(object msg)
        {
            Log(MessageType.Info, msg);
        }
    }
}

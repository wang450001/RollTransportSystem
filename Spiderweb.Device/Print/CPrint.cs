using Spiderweb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spiderweb.Device.Print
{
    public abstract class CPrint : CDevice
    {
        public string PrinterIp { get; protected set; }

        public string PrinterName { get; protected set; }

        protected CPrint() : base()
        {

        }

        protected CPrint(string connStr) : base(connStr)
        {

        }

        protected override void ParseConnectionChar(char split = '|')
        {
            if (string.IsNullOrEmpty(ConnectionChar)) return;

            string[] cnnStrs = ConnectionChar.Split(split);
            if (cnnStrs == null || cnnStrs.Length < 1) return;
            PrinterIp = cnnStrs[0];

            if (cnnStrs.Length > 1) PrinterName = cnnStrs[1];

            PingHost(PrinterIp);
        }

        public static new CPrint CreateInstance(string typeName, string connStr)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(connStr)) return null;

            return (CPrint)Activator.CreateInstance(Type.GetType(typeName), connStr);
        }
    } 
}

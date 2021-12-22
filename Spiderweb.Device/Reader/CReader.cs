using Spiderweb.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spiderweb.Device.Reader
{
    public class ReaderEventArgs
    {
        public string Barcode  { get; set; }

        public int ChannelIndex { get; set; }

        public ReaderEventArgs(string barcode, int channel)
        {
            this.Barcode = barcode;
            this.ChannelIndex = channel;
        }
    }


    public abstract class CReader : CDevice
    {
        public string ReaderIp { get; protected set; }

        public int ReaderPort { get; protected set; }

        public static new CReader CreateInstance(string typeName, string connStr)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(connStr)) return null;

            return (CReader)Activator.CreateInstance(Type.GetType(typeName), connStr);
        }

        protected CReader() : base()
        {

        }

        protected CReader(string connStr)
            : base(connStr)
        {

        }

        public virtual bool Stop()
        {
            return true;
        }

        protected virtual void OnReceiveData(string barcode, int channel)
        {
            base.OnReceiveData(new ReaderEventArgs(barcode, channel));
        }

        protected override void ParseConnectionChar(char split = '|')
        {
            if (string.IsNullOrEmpty(ConnectionChar)) return;

            string[] cnnStrs = ConnectionChar.Split(split);
            if (cnnStrs == null || cnnStrs.Length < 1) return;
            ReaderIp = cnnStrs[0];

            int temp = 0;
            if (cnnStrs.Length > 1) int.TryParse(cnnStrs[1], out temp);
            if (temp > 0) ReaderPort = temp;

            PingHost(ReaderIp);
        }
    }
}

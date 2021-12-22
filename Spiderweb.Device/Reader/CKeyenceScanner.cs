using Keyence.AutoID.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Reader
{
 /**
 * 功能：KEYENCE条码读码器(SR-5000)功能类
 * 
 * 日期：2021-12-21
 * 
 * 作者：Spiderweb
 * 
 * */
    public class CKeyenceScanner : CReader
    {
        ReaderAccessor reader;
        int channelId;

        public CKeyenceScanner() 
            : base()
        {
            ReaderIp = "192.168.100.100";
        }

        public CKeyenceScanner(string connStr) : base(connStr)
        {

        }

        protected override void Init()
        {
            reader = new ReaderAccessor();
            channelId = 0;
            base.Init();
        }

        public override void Dispose()
        {
            base.Dispose();

            reader.Dispose();
        }

        public override void Connect()
        {
            reader.IpAddress = ReaderIp;
            Connected = reader.Connect((data) =>
            {
                Working = false;
                stopwatch.Stop();

                string barcode = Encoding.ASCII.GetString(data);
                if (!string.IsNullOrEmpty(barcode)) OnReceiveData(barcode, channelId);
                OnSendMessage($"读码成功，读码<{barcode}>耗时{stopwatch.ElapsedMilliseconds}ms");
            });

            OnSendMessage($"连接读码器<{ReaderIp}>成功");
        }

        public override void Disconnect()
        {
            if (reader == null) return;

            if (Connected) reader.Disconnect();
            reader.Dispose();

            OnSendMessage($"断开读码器<{ReaderIp}>连接");
        }

        public override bool Read()
        {
            if (reader == null) return false;

            Working = true;
            stopwatch.Restart();

            string resp = reader.ExecCommand("LON");
            OnSendMessage($"打开读码器<{ReaderIp}:{ReaderPort}>，等待读码成功或停止");
            if (!string.IsNullOrEmpty(resp) && !resp.ToLower().Equals("error"))
                OnReceiveData(resp, channelId);

            return base.Read();
        }

        public override bool Stop()
        {
            OnSendMessage($"关闭读码器<{ReaderIp}>");
            if (Connected) reader.ExecCommand("LOFF");

            return base.Stop();
        }
    }
}

using Spiderweb.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spiderweb.Device
{
    public delegate void SendMessageEventHandler(object sender, string msg);

    public delegate void ReceiveDataEventHandler(object sender, object data);

    public delegate void ConnectChangedEventHandler(object sender, bool connected);

    public abstract class CDevice : IDisposable
    {
        protected Stopwatch stopwatch;
        protected CancellationTokenSource pingToken;

        public bool Connected { get; set; }

        public bool Working { get; set; }

        public string ConnectionChar { get; set; }

        public event SendMessageEventHandler SendMessage;

        public event ReceiveDataEventHandler ReceiveData;

        public event ConnectChangedEventHandler ConnectChanged;

        public static CDevice CreateInstance(string typeName, string connStr)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(connStr)) return null;

            return (CDevice)Activator.CreateInstance(Type.GetType(typeName), connStr);
        }

        protected CDevice()
        {
            Init();
        }

        protected CDevice(string connStr)
        {
            Init();

            this.ConnectionChar = connStr;
            ParseConnectionChar();
        }

        public virtual void Connect() { }

        public virtual void Disconnect() { }

        public virtual bool Read()
        {
            return true;
        }

        public virtual bool Write(params object[] obj)
        {
            return true;
        }

        protected virtual void Init()
        {
            stopwatch = new Stopwatch();
            pingToken = new CancellationTokenSource();
        }

        protected void PingHost(string ip)
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    //测试网络通断
                    bool isOk = CommonUtils.PingHost(ip);
                    if (!isOk.Equals(Connected))
                    {
                        Connected = isOk;
                        OnConnectChanged();
                    }

                    if (pingToken.IsCancellationRequested) return;

                    await Task.Delay(1000);
                }
            }, pingToken.Token);
        }

        protected virtual void ParseConnectionChar(char split = '|')
        {

        }

        protected virtual void OnSendMessage(string msg)
        {
            SendMessage?.Invoke(this, msg);
        }

        protected virtual void OnReceiveData(object data)
        {
            ReceiveData?.Invoke(this, data);
            //ReceiveData?.BeginInvoke(this, data, null, null);
        }

        protected virtual void OnConnectChanged()
        {
            ConnectChanged?.Invoke(this, Connected);
        }

        public virtual void Dispose()
        {
            if (pingToken == null) return;
            pingToken.Cancel();
            pingToken.Dispose();
            pingToken = null;
        }
    }
}

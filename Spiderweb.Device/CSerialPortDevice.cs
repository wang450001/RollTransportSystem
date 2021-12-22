using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device
{
    public abstract class CSerialPortDevice : CDevice
    {
        protected SerialPort port;
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBit { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBit { get; set; }

        protected CSerialPortDevice() : base()
        {
            InitPort();
        }

        protected CSerialPortDevice(string connStr) : base(connStr)
        {
            InitPort();
        }

        protected override void Init()
        {
            PortName = "COM1";
            BaudRate = 9600;
            DataBit = 8;
            Parity = Parity.None;
            StopBit = StopBits.One;

            base.Init();
        }

        void InitPort()
        {
            port = new SerialPort
            {
                PortName = PortName,
                BaudRate = BaudRate,
                DataBits = DataBit,
                Parity = Parity,
                StopBits = StopBit,

                Handshake = Handshake.None,
                ReceivedBytesThreshold = 1
            };
            port.DataReceived += Port_DataReceived; ;
        }

        protected virtual void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Connected = true;
            OnConnectChanged();
        }

        public override void Dispose()
        {
            base.Dispose();

            port.Dispose();
            port = null;
        }

        protected override void ParseConnectionChar(char split = '|')
        {
            if (string.IsNullOrEmpty(ConnectionChar)) return;

            string[] strParams = ConnectionChar.Split(split);
            if (strParams == null)
            {
                OnSendMessage("串口参数格式错误，数据示例：COM1,9600,8,N,1");
                return;
            }

            //提取端口名称
            if (strParams.Length > 0)
                this.PortName = strParams[0];

            //提取波特率
            if (strParams.Length > 1)
                BaudRate = Convert.ToInt32(strParams[1]);
            //提取数据位
            if (strParams.Length > 2)
                DataBit = Convert.ToInt32(strParams[2]);
            //提取校验
            if (strParams.Length > 2)
            {
                string strParity = strParams[3];
                switch (strParity.ToUpper())
                {
                    case "E":
                        Parity = Parity.Even;
                        break;
                    case "O":
                        Parity = Parity.Odd;
                        break;
                    default:
                        Parity = Parity.None;
                        break;
                }
            }

            if (strParams.Length > 4)
            {
                //提取停止位
                int.TryParse(strParams[4], out int intStopBit);
                StopBit = (StopBits)intStopBit;
            }
        }
    }
}

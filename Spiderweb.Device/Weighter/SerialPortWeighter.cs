using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Weighter
{
    public class SerialPortWeighter : CSerialWeighter
    {
        SerialPort port;
        string weighterString;

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBit { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBit { get; set; }

        protected SerialPortWeighter() : base()
        {

        }

        protected SerialPortWeighter(string connStr) : base(connStr)
        {

        }

        protected override void Init()
        {
            PortName = "COM1";
            BaudRate = 9600;
            DataBit = 8;
            Parity = Parity.None;
            StopBit = StopBits.One;

            port = new SerialPort();
            weighterString = "";
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
                int intStopBit = 1;
                int.TryParse(strParams[4], out intStopBit);
                StopBit = (StopBits)intStopBit;
            }
        }

        public override void Connect()
        {
            if (port == null) port = new SerialPort();

            port.PortName = PortName;
            port.BaudRate = BaudRate;
            port.DataBits = DataBit;
            port.Parity = Parity;
            port.StopBits = StopBit;

            port.Handshake = Handshake.None;
            port.ReceivedBytesThreshold = 1;
            port.DataReceived += Port_DataReceived;

            try
            {
                port.Open();
                OnSendMessage($"称重仪表端口<{PortName}>打开成功");
            }
            catch (Exception ex)
            {
                OnSendMessage($"称重仪表端口<{PortName}>打开失败，{ex.Message}");
                if (port != null && port.IsOpen) port.Close();
            }
            finally
            {
                Connected = port.IsOpen;
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string strRead = "";
            float curWeight = 0f;
            try
            {
                //获取串口接收到的字符串
                strRead = port.ReadExisting();

                weighterString += strRead;
                //没有设置称重仪表数据协议则不解析称重数据
                if (WeighterProtocol == null) return;

                if (WeighterProtocol.GetWeight(weighterString, ref curWeight))
                {
                    this.WeighterRawData = weighterString;
                    OnSendMessage($"仪表显示重量：{curWeight}Kg/{WeighterRawData}");
                    OnReceiveData(curWeight);
                    weighterString = "";
                }
            }
            catch (Exception ex)
            {
                OnSendMessage($"接收仪表数据错误，{ex.Message}");
                weighterString = "";
                if(port.IsOpen)
                {
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                }
            }
        }

        public override void Disconnect()
        {
            if (port == null) return;
            port.DataReceived -= Port_DataReceived;
            port.Close();

            Connected = false;
            OnSendMessage($"称重仪表端口<{PortName}>关闭成功");
        }
    }
}

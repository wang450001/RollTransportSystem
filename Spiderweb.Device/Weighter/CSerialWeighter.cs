using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Weighter
{
    public abstract class CSerialWeighter : CSerialPortDevice
    {
        string weighterString;
        protected string WeighterRawData { get; set; }

        public IWeighterProtocol WeighterProtocol { get; protected set; }

        protected CSerialWeighter() : base()
        {
            weighterString = "";
        }

        protected CSerialWeighter(string connStr) : base(connStr)
        {
            weighterString = "";
        }

        public override void Connect()
        {
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

        public override void Disconnect()
        {
            if (port == null) return;
            port.DataReceived -= Port_DataReceived;
            port.Close();

            Connected = false;
            OnSendMessage($"称重仪表端口<{PortName}>关闭成功");
        }

        protected override void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            float curWeight = 0f;
            try
            {
                //获取串口接收到的字符串
                string strRead = port.ReadExisting();

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
                if (port.IsOpen)
                {
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                }
            }
            finally
            {
                base.Port_DataReceived(sender, e);
            }
        }

        public static new CSerialWeighter CreateInstance(string typeName, string connStr)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(connStr)) return null;

            return (CSerialWeighter)Activator.CreateInstance(Type.GetType(typeName), connStr);
        }

    }
}

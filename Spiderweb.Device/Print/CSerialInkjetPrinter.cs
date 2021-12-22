using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Print
{
    public abstract class CSerialInkjetPrinter : CSerialPortDevice
    {
        public IPrintProtocol PrintProtocol { get; set; }

        protected CSerialInkjetPrinter() : base()
        {

        }

        protected CSerialInkjetPrinter(string connStr) : base(connStr)
        {

        }

        public static new CSerialInkjetPrinter CreateInstance(string typeName, string connStr)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(connStr)) return null;

            return (CSerialInkjetPrinter)Activator.CreateInstance(Type.GetType(typeName), connStr);
        }

        public override void Connect()
        {
            try
            {
                port.Open();
                OnSendMessage($"喷码机端口<{PortName}>打开成功");
            }
            catch (Exception ex)
            {
                OnSendMessage($"喷码机端口<{PortName}>打开失败，{ex.Message}");
                if (port != null && port.IsOpen) port.Close();
            }
            finally
            {
                Connected = port.IsOpen;
                OnConnectChanged();
            }
        }

        public override void Disconnect()
        {
            if (port == null) return;
            port.DataReceived -= Port_DataReceived;
            port.Close();

            Connected = false;
            OnConnectChanged();
            OnSendMessage($"喷码机端口<{PortName}>关闭成功");
        }

        public override bool Write(params object[] data)
        {
            if (data == null)
            {
                OnSendMessage("待喷印数据为空，请检查");
                return false;
            }            

            if (PrintProtocol == null)
            {
                OnSendMessage("喷印组串协议为空，请检查");
                return false;
            }

            string msg = PrintProtocol.GetSendCommand(data);
            if (string.IsNullOrEmpty(msg))
            {
                OnSendMessage($"喷码机组串字符为空");
                return false;
            }

            if (port.IsOpen)
            {
                port.Write(msg);
                OnSendMessage($"下发喷印数据<{msg}>成功");
            }
            else
            {
                OnSendMessage($"喷码机端口<{PortName}>没有打开");
            }
            

            return true;
        }

        protected override void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //获取喷码机响应数据
            string msg = port.ReadExisting();

            if (PrintProtocol == null)
            {
                OnSendMessage("喷印组串协议为空，请检查");
                return;
            }

            bool bRet = PrintProtocol.GetResponseStatus(msg);
            OnSendMessage($"喷码机响应数据{msg}，接收喷印刷数据{(bRet ? "成功" : "失败")}");

            base.Port_DataReceived(sender, e);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Print
{
    public class SerialPortInkjet : CPrint
    {
        SerialPort port;

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBit { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBit { get; set; }

        public SerialPortInkjet() : base()
        {

        }

        public SerialPortInkjet(string connStr) : base(connStr)
        {

        }
    }
}

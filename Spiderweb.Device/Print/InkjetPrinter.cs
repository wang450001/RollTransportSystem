using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Print
{
    /**
     * Author: Spiderweb
     * 
     * Date: 2021-12-22
     * 
     * Function: 各喷码机实体类
     * 
     * */
    public class KarmaInkjetPrinter : CSerialInkjetPrinter
    {
        public KarmaInkjetPrinter() : this("")
        {

        }

        public KarmaInkjetPrinter(string connStr) : base(connStr)
        {
            this.PrintProtocol = new KarmaInkjetPrintProtocol();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Author: spiderweb
 * 
 * Date: 2021-12-22
 * 
 * Function: 各品牌的称重仪表类
 * 
 * */
namespace Spiderweb.Device.Weighter
{

    public class ToledoWeighter : CSerialWeighter
    {
        public ToledoWeighter() : this("")
        {

        }

        public ToledoWeighter(string connStr) : base(connStr)
        {
            WeighterProtocol = new ToledoWeighterProtocol();
        }
    }

    public class KunhongWeighter : CSerialWeighter
    {
        public KunhongWeighter() : this("")
        {

        }

        public KunhongWeighter(string connStr) : base(connStr)
        {
            WeighterProtocol = new KunhongWeighterProtocol();
        }
    }

    public class YingzanWeighter : CSerialWeighter
    {
        public YingzanWeighter() : this("")
        {

        }

        public YingzanWeighter(string connStr) : base(connStr)
        {
            WeighterProtocol = new YingzanWeighterProtocol();
        }
    }
}

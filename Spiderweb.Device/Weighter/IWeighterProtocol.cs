using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Weighter
{
    public interface IWeighterProtocol
    {
        /// <summary>
        /// 获取称重重量
        /// </summary>
        /// <param name="rawString">称重仪表原始字符数据</param>
        /// <param name="weight"></param>
        /// <returns>称重重量，单位Kg</returns>
        bool GetWeight(string rawString, ref float weight);
    }
}

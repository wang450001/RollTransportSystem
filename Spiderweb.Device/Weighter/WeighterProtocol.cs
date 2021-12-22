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
 * Function: 各品牌称重仪表数据解析协议接口实现类
 * 
 * */
namespace Spiderweb.Device.Weighter
{
    public class ToledoWeighterProtocol : IWeighterProtocol
    {
        public bool GetWeight(string rawString, ref float weight)
        {
            int dp = 0;//小数点位数
            bool boolResult = false;

            if (string.IsNullOrEmpty(rawString))
            {
                weight = 0;
                return boolResult;
            }

            string strTemp = rawString.ToUpper();
            try
            {
                if (strTemp[0] == (char)2/*起始位*/ && (strTemp.Length == 17 || strTemp.Length == 18))
                {
                    //获取状态字A，状态字A用3位表示小数点位置
                    //0表示*100，1表示*10，2表示*1，3表示*0.1，4表示*0.01
                    dp = Convert.ToChar(strTemp[1] & 07);//&07是把原来+16获得的符号转换为数字

                    //获取显示重量
                    weight = (float)(Convert.ToDouble(strTemp.Substring(4, 6)) * System.Math.Pow(10, 2 - dp));

                    //rawString = "";
                    boolResult = true;
                }
            }
            catch
            {
                boolResult = false;
            }

            return boolResult;
        }
    }

    public class KunhongWeighterProtocol : IWeighterProtocol
    {
        public bool GetWeight(string rawString, ref float weight)
        {
            //int dp = 0;//小数点位数
            bool boolResult = false;

            if (string.IsNullOrEmpty(rawString))
            {
                weight = 0;
                return boolResult;
            }

            string strTemp = rawString.ToUpper();
            try
            {
                //==============================称重协议===================================
                //--------------------------------------------------------------------------
                //| S | T | , | N | T| , | -1 | 2 | 3 | 4 | . | 5 | 6 |  | k | g | CR | LF |
                //--------------------------------------------------------------------------
                //| Header1	  | Header2	 |  Data（8 digits in length）|  Unit    |         |
                //--------------------------------------------------------------------------

                if ((strTemp[0] == 'S' || strTemp[0] == 'U' || strTemp[0] == 'O')/*起始位*/ && strTemp.Length >= 14)
                {
                    //获取显示重量
                    //称重仪表的单位需设置为Kg
                    float.TryParse(strTemp.Substring(6, 8), out weight);

                    //rawString = "";
                    boolResult = true;
                }
            }
            catch
            {
                boolResult = false;
            }

            return boolResult;
        }
    }

    public class YingzanWeighterProtocol : IWeighterProtocol
    {
        public bool GetWeight(string rawString, ref float weight)
        {
            //int dp = 0;//小数点位数
            bool boolResult = false;

            if (string.IsNullOrEmpty(rawString))
            {
                weight = 0;
                return boolResult;
            }

            string strTemp = rawString.ToUpper();
            try
            {
                //==============================称重协议===================================
                //--------------------------------------------------------------------------
                //| S | T | , | N | T| , | -1 | 2 | 3 | 4 | . | 5 | 6 |  | k | g | CR | LF |
                //--------------------------------------------------------------------------
                //| Header1	  | Header2	 |  Data（8 digits in length）|  Unit    |         |
                //--------------------------------------------------------------------------

                if ((strTemp[0] == 'S' || strTemp[0] == 'U' || strTemp[0] == 'O')/*起始位*/ && strTemp.Length >= 14)
                {
                    //获取显示重量
                    //称重仪表的单位需设置为Kg
                    float.TryParse(strTemp.Substring(7, 7), out weight);

                    //rawString = "";
                    boolResult = true;
                }
            }
            catch
            {
                boolResult = false;
            }

            return boolResult;
        }
    }
}

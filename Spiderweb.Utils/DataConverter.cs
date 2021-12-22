using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Utils
{
    public class DataConverter
    {
        public static bool ToBool(string str)
        {
            bool bRet = false;

            bool.TryParse(str, out bRet);

            return bRet;
        }

        /// <summary>
        /// 获取数据指定Bit位的值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="index">指定Bit位</param>
        /// <returns>数据指定Bit位的布尔状态</returns>
        public static bool GetBool(int data, int index)
        {
            int tmp = 1 << index;
            int tmpData = data & tmp;
            return tmpData > 0;
        }

        /// <summary>
        /// 获取数据的二进制
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool[] GetBool(int data)
        {
            bool[] bRes = new bool[32];
            for (int i = 0; i < bRes.Length; i++)
                bRes[i] = GetBool(data, i);

            return bRes;
        }

        public static byte ToByte(object data)
        {
            byte byteRet = 0;

            if (null != data)
                byte.TryParse(data.ToString(), out byteRet);

            return byteRet;
        }

        public static byte ToByte(bool[] bln)
        {
            if (null == bln || bln.Length == 0) return 0;

            int intLen = bln.Length > 8 ? 8 : bln.Length;
            int tmpRes = 0;

            for (int i = 0; i < intLen; i++)
            {
                if(bln[i])
                tmpRes += (1 << i);
            }

            return (byte)tmpRes;
        }

        public static short ToShort(object data)
        {
            short shtRet = 0;

            if (null != data)
                short.TryParse(data.ToString(), out shtRet);

            return shtRet;
        }

        public static short ToShort(bool[] bln)
        {
            if (null == bln || bln.Length == 0) return 0;

            int intLen = bln.Length > 16 ? 16 : bln.Length;
            int tmpRes = 0;

            for (int i = 0; i < intLen; i++)
            {
                if (bln[i])
                    tmpRes += (int)(1 << i);
            }

            return (short)tmpRes;
        }

        public static int ToInt(object data)
        {
            int intRet = 0;

            if (null != data)
                int.TryParse(data.ToString(), out intRet);

            return intRet;
        }

        public static int ToInt(bool[] bln)
        {
            if (null == bln || bln.Length == 0) return 0;

            int intLen = bln.Length > 32 ? 32 : bln.Length;
            int tmpRes = 0;

            for (int i = 0; i < intLen; i++)
            {
                if (bln[i])
                    tmpRes += (int)(1 << i);
            }

            return tmpRes;
        }

        public static long ToLong(object data)
        {
            long intRet = 0L;

            if (null != data)
                long.TryParse(data.ToString(), out intRet);

            return intRet;
        }

        public static float ToFloat(object data)
        {
            float fltRet = 0f;

            if (null != data)
                float.TryParse(data.ToString(), out fltRet);

            return fltRet;
        }

        public static DateTime ToDateTime(string data)
        {
            DateTime dt = DateTime.Now.Date;
            DateTime.TryParse(data, out dt);

            return dt;
        }
    }
}

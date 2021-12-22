using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Utils
{
    public static class CommonUtils
    {
        static Color green, red;

        static CommonUtils()
        {
            green = Color.FromArgb(0, 255, 0);
            red = Color.FromArgb(255, 0, 0);
        }

        /// <summary>
        /// 测试网络通断
        /// </summary>
        /// <param name="ip">主机IP地址</param>
        /// <returns></returns>
        public static bool PingHost(string ip)
        {
            try
            {
                PingReply reply = new Ping().Send(ip, 1000);
                if (reply == null) return false;
                return reply.Status.ToString().Equals("Success");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="data">待转换的数组</param>
        /// <returns></returns>
        public static string ToString(byte[] data)
        {
            if (data == null) return "空";
            return String.Join(" ", data);
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="toBase"></param>
        /// <returns></returns>
        public static string ToString(byte[] data, int toBase)
        {
            string strTemp = "";
            foreach (int tmp in data)
            {
                strTemp += Convert.ToString(tmp, toBase) + " ";
            }
            return strTemp;
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="data">待转换的数组</param>
        /// <returns></returns>
        public static string ToString(uint[] data)
        {
            return string.Join(" ", data);
        }

        /// <summary>
        /// 将uint数组转换为字节数组
        /// </summary>
        /// <param name="data">待转换的数组</param>
        /// <returns></returns>
        public static byte[] GetBytes(uint[] data)
        {
            const int UINT_BYTES_LENGTH = 4;
            if (data == null || data.Length < 1) return null;

            byte[] retBytes = new byte[data.Length * UINT_BYTES_LENGTH];
            byte[] tmpBytes = new byte[UINT_BYTES_LENGTH];

            for (int i = 0; i < data.Length; i++)
            {
                tmpBytes = BitConverter.GetBytes(data[i]);
                Array.Copy(tmpBytes, 0, retBytes, UINT_BYTES_LENGTH * i, UINT_BYTES_LENGTH);
            }

                return retBytes;
        }

        /// <summary>
        /// 判断目标是文件夹还是目录(目录包括磁盘)
        /// </summary>
        /// <param name="filepath">文件名</param>
        /// <returns></returns>
        public static bool IsDir(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            if ((fi.Attributes & FileAttributes.Directory) != 0)
                return true;
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            //获取主机名
            string hostName = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostEntry(hostName);

            foreach (IPAddress ipEntry in localhost.AddressList)
            {
                if (ipEntry.AddressFamily == AddressFamily.InterNetwork)
                    return ipEntry.ToString();               
            }
            return null;
        }

        /// <summary>
        /// 获取本机的MAC
        /// </summary>
        /// <returns></returns>
        public static string getLocalMac()
        {
            string mac = null;
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    mac = mo["MacAddress"].ToString();
            }
            return (mac);
        }

        public static string GetLocalIp(string adapterName)
        {
            if (string.IsNullOrEmpty(adapterName)) return null;

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            if (nics == null || nics.Length < 1) return null;

            foreach (NetworkInterface adapter in nics)
            {
                if (!adapter.Name.Equals(adapterName)) continue;
                IPInterfaceProperties ipip = adapter.GetIPProperties();
                if (ipip == null || ipip.UnicastAddresses == null || ipip.UnicastAddresses.Count < 1) continue;
                foreach (UnicastIPAddressInformation uii in ipip.UnicastAddresses)
                {
                    if (uii.Address.AddressFamily != AddressFamily.InterNetwork) continue;
                    //采用IPV4
                    return uii.Address.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// 获取连接的本地IP地址
        /// </summary>
        /// <param name="socket">连接套接字</param>
        /// <returns></returns>
        public static string GetLocalIp(Socket socket)
        {
            string ip = string.Empty;
            if (socket == null) return ip;

            try
            {
                IPEndPoint ipPoint = (IPEndPoint)socket.LocalEndPoint;
                ip = ipPoint.Address.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ip;
        }

        /// <summary>
        /// 获取连接的远程IP地址
        /// </summary>
        /// <param name="socket">连接套接字</param>
        /// <returns></returns>
        public static string GetRemoteIp(Socket socket)
        {
            string ip = string.Empty;
            if (socket == null) return ip;

            try
            {
                IPEndPoint ipPoint = (IPEndPoint)socket.RemoteEndPoint;
                ip = ipPoint.Address.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ip;
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime dt)
        {
            return FormatDate(dt, null);
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <returns></returns>
        public static string FormatDate()
        {
            return FormatDate(DateTime.Now);
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="dateType">日期类型</param>
        /// <returns></returns>
        public static string FormatDate(int? dateType)
        {
            return FormatDate(DateTime.Now, dateType);
        }

        
        /// <summary>
        /// 日期格式化
        /// </summary>
        ///<param name="dt">日期对象</param>
        ///<param name="dateType">日期类型</param>
        /// <returns></returns>
        public static string FormatDate(DateTime dt, int? dateType)
        {
            if (null == dt) return null;

            switch (dateType)
            {
                case 2:
                    return dt.ToString("yy.MM.dd");
                case 102:
                    return dt.ToString("yyyy.MM.dd");

                case 11:
                    return dt.ToString("yy/MM/dd");
                case 111:
                    return dt.ToString("yyyy/MM/dd");

                case 12:
                    return dt.ToString("yyMMdd");
                case 112:
                    return dt.ToString("yyyyMMdd");

                case 23:
                    return dt.ToString("yyyy-MM-dd");
                case 123://SQL SERVER 无此DateType
                    return dt.ToString("yy-MM-dd");

                case 24:
                    return dt.ToString("HH:mm:ss");
                case 124://SQL SERVER 无此DateType
                    return dt.ToString("HHmmss");

                case 50://SQL SERVER 无此DateType
                    return dt.ToString("yyMMddHHmmss");
                case 150://SQL SERVER 无此DateType
                    return dt.ToString("yyyyMMddHHmmss");

                case 201:
                    return dt.Ticks.ToString();
                case 202:
                    

                case 20:
                case 120:
                default:
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 网络正常连接颜色
        /// </summary>
        public static Color COLOR_CONN
        {
            get { return green; }
        }

        /// <summary>
        /// 网络断开连接颜色
        /// </summary>
        public static Color COLOR_DISCNN
        {
            get { return red; }
        }

        public static string ConvertToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}

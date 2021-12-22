using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Print
{
    public interface IPrintProtocol
    {
        /// <summary>
        /// 使用喷码机协议包装待喷印数据
        /// </summary>
        /// <param name="obj">待喷印数据</param>
        /// <returns></returns>
        string GetSendCommand(params object[] obj);

        /// <summary>
        /// 获取喷码机响应状态
        /// </summary>
        /// <param name="msg">喷码机响应数据</param>
        /// <returns></returns>
        bool GetResponseStatus(string msg);
    }
}

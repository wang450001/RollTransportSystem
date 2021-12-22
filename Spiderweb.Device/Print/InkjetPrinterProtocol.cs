using Spiderweb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Print
{
    public class KarmaInkjetPrintProtocol : IPrintProtocol
    {
        /// <summary>
        /// 解析反馈数据
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetResponseStatus(string msg)
        {
            return msg.Contains("@file ok");
        }

        /// <summary>
        /// 包装数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetSendCommand(params object[] obj)
        {
            /**
             * 
             * {"message":[
             * 
             * {"type":"text", "content":"PT 2150","X":0, "Y":0,"FONT":16, "BOLD":0,"GAP":0},
             * {"type":"text", "content":"mm","X":58, "Y":8,"FONT":7, "BOLD":0,"GAP":1},
             * {"type":"text", "content":"0001-1","X":78, "Y":0,"FONT":16, "BOLD":0,"GAP":0},
             * {"type":"text", "content":"no","X":128, "Y":8,"FONT":7, "BOLD":0,"GAP":1},
             * {"type":"text", "content":"1","X":150, "Y":0,"FONT":16, "BOLD":0,"GAP":0},
             * {"type":"text", "content":"kg","X":160, "Y":8,"FONT":7, "BOLD":0,"GAP":1},
             * {"type":"text", "content":"kg","X":160, "Y":8,"FONT":7, "BOLD":0,"GAP":1},
             * {"type":"text", "content":"T26190 JR","X":180, "Y":0,"FONT":7, "BOLD":0,"GAP":1},
             * {"type":"text", "content":"18/7 21:53","X":180, "Y":8,"FONT":7, "BOLD":0,"GAP":1}
             * 	]
             * }
             * 
             * 
             * 通讯格式：json 文件格式 字符均为ASCII格式，请严格遵循。
             * 接口：    RS232 
             * 波特率：  9600,14400,19200,38400,57600,115200 可在系统设置里面更改，更改后重启设备
             * 校验位：  无
             * 数据位：  8
             * 停止位：  1
             * 备注：
             * 1、同一帧数据内字符与字符发送延时不超过2ms，否则认为为两帧数据，这样json解析会失败。
             * 2、文件最大长度为1K字节，超出则认为为两帧数据，这样json解析会失败。
             * 
             * 
             * 设备收到文件后解析：
             * 正确返回："@file ok!"
             * 错误返回："@File error!"
             * 
             * 
             * 文件解析：
            *  type: 文本格式 text
            *  content: 喷印内容
            *  X：  content 横坐标
            *  Y:   content 纵坐标
            *  FONT： content字体 16：8*16  7：5*7
            *  BOLD:  content加粗 （0-10）
            *  GAP：  content字间距 （0-10）
             * 
             * */

            if (obj == null || obj.Length < 2) return "";
            string[] convMsgs = new string[obj.Length + 1];

            convMsgs[0] = CommonUtils.ConvertToJson(new KarmaInkjetMessage(obj[0].ToString()));
            convMsgs[1] = CommonUtils.ConvertToJson(new KarmaInkjetMessage(obj[1].ToString(), 180, 0, 7));
            convMsgs[2] = CommonUtils.ConvertToJson(new KarmaInkjetMessage(DateTime.Now.ToString("yy/MM/dd"), 180, 8, 7));

            string msg = $"{{\"message\":[{string.Join(",", convMsgs) }]}}";

            return msg;
        }

        class KarmaInkjetMessage
        {
            public string type { get; set; }

            public string content { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public int FONT { get; set; }

            public int BOLD { get; set; }

            public int GAP { get; set; }

            public KarmaInkjetMessage()
            {
                type = "text";
                FONT = 16;
            }

            public KarmaInkjetMessage(string content)
                : this()
            {
                this.content = content;
            }

            public KarmaInkjetMessage(string content, int x, int y)
                : this(content)
            {
                this.X = x;
                this.Y = y;
            }

            public KarmaInkjetMessage(string content, int x, int y, int size)
                : this(content, x, y)
            {
                this.FONT = size;
            }
        }
    }
}

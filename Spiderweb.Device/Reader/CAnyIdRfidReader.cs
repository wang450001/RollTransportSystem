using RFIDStation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spiderweb.Device.Reader
{
    /**
     * 功能：上海普阅Rfid阅读器功能类
     * 
     * 日期：2021-12-21
     * 
     * 作者：Spiderweb
     * 
     * */
    public class CAnyIdRfidReader : CReader
    {
        int deviceHandle;//设备句柄
        ushort deviceAddress;//设备地址

        List<Tuple<byte/*天线号*/, byte/*读取次数*/>> listAntenna;
        CancellationTokenSource tokenSrc;
        UHFREADER_INVTPARAMS inventoryParams;//Rfid读码参数

        string strEpc;//读取的EPC
        int antennaId;//读取的天线Id

        public CAnyIdRfidReader() 
            : base()
        {
            ReaderIp = "192.168.10.21";
            ReaderPort = 10001;
        }

        /// <summary>
        /// 连接字符串格式:阅读器IP，阅读器端口，所用天线号
        /// 示例：192.168.10.10,10001,0,1
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public CAnyIdRfidReader(string connStr) 
            : base(connStr) { }

        protected override void Init()
        {
            deviceAddress = 0x0001;

            inventoryParams = new UHFREADER_INVTPARAMS
            {
                antIndex = new Byte[uhfReaderDll.UHFREADER_ANT_NUM],
                antRepeat = new Byte[uhfReaderDll.UHFREADER_ANT_NUM],
                epc = new UHFREADER_EPC[256]
            };
            for (int i = 0; i < inventoryParams.epc.Length; i++)
            {
                inventoryParams.epc[i].epc = new Byte[uhfReaderDll.UHFREADER_EPC_LEN];
            }

            tokenSrc = new CancellationTokenSource();
            base.Init();
        }

        public override void Dispose()
        {
            base.Dispose();

            tokenSrc.Cancel();
            tokenSrc.Dispose();
            tokenSrc = null;
        }

        protected override  void ParseConnectionChar(char split='|')
        {
            /// 连接字符串格式:阅读器IP+阅读器端口+所用天线号
            /// 示例：192.168.10.10|10001|0|1
            ///解析连接字符
            if (string.IsNullOrEmpty(ConnectionChar)) return;

            string[] cnnStrs = ConnectionChar.Split(split);
            if (cnnStrs == null || cnnStrs.Length < 1) return;
            ReaderIp = cnnStrs[0];

            int temp = 0;
            if (cnnStrs.Length > 1) int.TryParse(cnnStrs[1], out temp);
            if (temp > 0) ReaderPort = temp;

            if (cnnStrs.Length < 3) return;
            listAntenna = new List<Tuple<byte, byte>>();
            for (int i = 2; i < cnnStrs.Length; i++)
            {
                if (int.TryParse(cnnStrs[i], out temp))
                    listAntenna.Add(new Tuple<byte, byte>((byte)temp, 1));
            }
        }

        public override void Connect()
        {
            try
            {
                deviceHandle = uhfReaderDll.uhfReaderOpenSocket(ReaderIp, (ushort)ReaderPort);

                Connected = !(deviceHandle == -1 || deviceHandle == 0);
            }
            catch (Exception ex)
            {
                OnSendMessage($"打开阅读器失败，原因:{ex.Message}");
            }      
        }

        public override void Disconnect()
        {
            try
            {
                if (deviceHandle <= 0) return;
                int handle = uhfReaderDll.uhfReaderCloseSocket(deviceHandle);

                deviceHandle = 0;
                Connected = false;
            }
            catch (Exception ex)
            {
                OnSendMessage($"关闭阅读器失败，原因:{ex.Message}");
            }
        }

        public override bool Read()
        {
            if (listAntenna == null) return false;
            if (!Connected) Connect();

            Task.Factory.StartNew(OpenReader, tokenSrc.Token)
                .ContinueWith((t) =>
                {
                    OnSendMessage($"阅读器读取结束");
                    Disconnect();
                });
            
            return base.Read();
        }

        public override bool Stop()
        {
            tokenSrc.Cancel();
            return base.Stop();
        }

        void OpenReader()
        {
            this.Working = true;

            byte[] tx = new byte[4096];
            byte[] rx = new byte[4096];

            try
            {
                inventoryParams.antNum = 0;
                for (int i = 0; i < listAntenna.Count; i++)
                {
                    //设置启用的天线
                    inventoryParams.antIndex[inventoryParams.antNum] = listAntenna[i].Item1;
                    //设置读取次数
                    inventoryParams.antRepeat[inventoryParams.antNum] = listAntenna[i].Item2;
                    inventoryParams.antNum++;
                }
                inventoryParams.to = 2000;//超时时间   

                int sendLen = 0;
                while (true)
                {
                    stopwatch.Restart();
                    inventoryParams.tagNum = 0;
                    sendLen = uhfReaderDll.uhfReaderInventory(deviceHandle, 0x0000, deviceAddress, ref inventoryParams, tx, rx);
                    OnSendMessage($"请求盘点返回数据：{sendLen}，句柄：{deviceHandle}");
                    stopwatch.Stop();
                    Thread.Sleep(10);

                    if (sendLen == 0)
                    {
                        OnSendMessage("读取标签失败");
                        return;
                    }
                    if (inventoryParams.result.flag > 0)
                    {
                        OnSendMessage($"读取标签错，,{DisplayOpResult(inventoryParams.result)}");
                        return;
                    }

                    OnSendMessage($"请求帧:{Hex2Str(tx, 0, tx[uhfReaderDll.UHFREADER_FRAME_POS_LEN] + 3)}，" +
                        $"读取{inventoryParams.tagNum}枚标签，耗时{stopwatch.ElapsedMilliseconds}ms");

                    if (inventoryParams.tagNum == 0)
                    {
                        OnSendMessage("未读取到标签");
                        return;
                    }

                    int i = 0;
                    string epcStr = "";

                    strEpc = Hex2Str(inventoryParams.epc[0].epc, 0, inventoryParams.epc[0].len);
                    for (i = 0; i < inventoryParams.tagNum; i++)
                    {
                        Byte[] ant = new Byte[1];
                        Byte[] rssi = new Byte[4];
                        UHFREADER_OPRESULT uhfResult = new UHFREADER_OPRESULT();

                        stopwatch.Restart();
                        int rlt = uhfReaderDll.uhfReaderGetTagInfo(deviceHandle, 0x0000, deviceAddress, (Byte)i, ant, rssi, ref uhfResult, tx, rx);
                        stopwatch.Stop();
                        OnSendMessage($"获取标签数据耗时:{ stopwatch.ElapsedMilliseconds}ms");

                        Thread.Sleep(10);
                        epcStr = string.Format("Epc:{0},长度:{1}", Hex2Str(inventoryParams.epc[i].epc, 0, inventoryParams.epc[i].len), inventoryParams.epc[i].len);
                        if (rlt > 0)
                        {
                            epcStr += $" 天线:{ant[0]}#,rssi:{rssi[0]}/{rssi[1]}/{rssi[2]}/{rssi[3]}";
                            antennaId = ant[0] + 1;
                        }
                    }
                    OnSendMessage(epcStr);

                    if (inventoryParams.tagNum == 1)
                        OnReceiveData(strEpc, antennaId);
                    else
                        OnSendMessage("读取的标签过多(大于1张标签)");

                    if (tokenSrc.Token.IsCancellationRequested) return;
                }
            }
            catch (Exception ex)
            {
                OnSendMessage($"{ex.Message}\r\n{ex.StackTrace}");
            }
            finally { Working = false; }
        }

        string Hex2Str(Byte[] info, int start, int len)
        {
            string str = "";

            if (info == null || info.Length < 1) return str;

            for (int i = 0; i < len; i++)
            {
                str += info[start + i].ToString("X").PadLeft(2, '0');
            }

            return str;
        }

        string DisplayOpResult(UHFREADER_OPRESULT result)
        {
            string infStr = "";
            if (result.flag == 0)
            {
                infStr = "<操作成功>\r\n\r\n" + infStr;
            }
            else
            {
                if (result.errType == 4)
                {
                    infStr = "<操作失败：参数错误或设备不支持该命令>\r\n\r\n" + infStr;
                }
                else if (result.errType == 3)
                {
                    infStr = "<操作失败：标签无响应>\r\n\r\n" + infStr;
                }
                else if (result.errType == 2)
                {
                    infStr = "<操作失败：设备忙>\r\n\r\n" + infStr;
                }
                else if (result.errType == 1)
                {
                    infStr = "<操作失败：标签错误>\r\n\r\n" + infStr;
                }
                else
                {
                    infStr = "<操作失败>\r\n\r\n" + infStr;
                }
            }
            return infStr;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_OPRESULT
{
    public uint srcAddr;
    public uint targetAddr;
    public uint flag;
    public uint errType;
    public uint t;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_FRAME
{
    public uint txLen;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
    public Byte[] txFrame;

    public uint rxLen;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
    public Byte[] rxFrame;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_RCVFRAME
{
    public uint state;
    public uint index;
    public uint len;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
    public Byte[] buffer;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_VERSION
{
    public UHFREADER_OPRESULT result;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public Byte[] type;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public Byte[] sv;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public Byte[] hv;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_DTUFRAME
{
    public uint cmd;
    public uint txLen;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
    public Byte[] txFrame;

    public uint rxLen;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
    public Byte[] rxFrame;

    public uint timeout;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_SYSPARAMS
{
    public UHFREADER_OPRESULT result;
    public uint addr;
    public uint buzzer;
    public uint br;
    public uint workMode;
    public uint cmdMode;
    public uint scanAntNum;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Byte[] scanAntAddr;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Byte[] scanAntRepeat;

    public uint scanWorkTime;
    public uint scanIdleTime;
    public uint scanRltFlag;
    public uint scanRltCom;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_ANTPARAMS
{
    public UHFREADER_OPRESULT result;
    public uint antNum;
    public uint power;
    public uint region;
    public uint regionParams0;
    public uint regionParams1;
    public uint regionParams2;
    public uint rtlThrd;
    public uint link;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_EPC
{
    public int len;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public Byte[] epc;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_INVTPARAMS
{
    public uint to;
    public uint antNum;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Byte[] antIndex;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Byte[] antRepeat;

    public uint tagNum;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public UHFREADER_EPC[] epc;

    public UHFREADER_OPRESULT result;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_RMEMPARAMS
{
    public uint to;//操作阅读器超时时间，单位：
    public uint bank;//区域：0、Reserved，1、EPC，2、TID，3、USR
    public uint startAddr; //开始地址
    public uint wordNum;//字节数

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Byte[] pwd; //密码

    public uint tagNum; //盘点标签数目

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public UHFREADER_EPC[] epc;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256 * 32)]
    public ushort[] memory;

    public UHFREADER_OPRESULT result;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_WMEMPARAMS
{
    public uint to;
    public uint bank;
    public uint startAddr;
    public uint wordNum;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Byte[] pwd;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public ushort[] memory;

    public uint tagNum;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public UHFREADER_EPC[] epc;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public Byte[] opRlt;

    public UHFREADER_OPRESULT result;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_LOCKPARAMS
{
    public uint to;
    public uint bank;
    public uint type;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Byte pwd;

    public uint tagNum;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public UHFREADER_EPC[] epc;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public Byte opRlt;

    public UHFREADER_OPRESULT result;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_KILLPARAMS
{
    public uint to;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Byte pwd;

    public uint tagNum;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public UHFREADER_EPC[] epc;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public Byte opRlt;

    public UHFREADER_OPRESULT result;
};

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
public struct UHFREADER_MASKPARAMS
{
    public uint len;
    public uint target;
    public uint action;
    public uint bank;
    public uint startAddr;
    public uint bitsNum;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public Byte[] mask;
    public UHFREADER_OPRESULT result;
};

namespace RFIDStation
{
    public class uhfReaderDll
    {
        public const byte UHFREADER_FRAME_HEAD1 = 0x7E;
        public const byte UHFREADER_FRAME_HEAD2 = 0x55;

        public const byte UHFREADER_FRAME_POS_HEAD1 = 0;
        public const byte UHFREADER_FRAME_POS_HEAD2 = 1;
        public const byte UHFREADER_FRAME_POS_LEN = 2;
        public const byte UHFREADER_FRAME_POS_SRCAddr = 3;
        public const byte UHFREADER_FRAME_POS_DESTADDR = 5;
        public const byte UHFREADER_FRAME_POS_CMD = 7;
        public const byte UHFREADER_FRAME_POS_RFU = 8;
        public const byte UHFREADER_FRAME_POS_PAR = 9;

        public const byte UHFREADER_RFRAME_POS_HEAD1 = 0;
        public const byte UHFREADER_RFRAME_POS_HEAD2 = 1;
        public const byte UHFREADER_RFRAME_POS_LEN = 2;
        public const byte UHFREADER_RFRAME_POS_SRCADDR = 3;
        public const byte UHFREADER_RFRAME_POS_DESTADDR = 5;
        public const byte UHFREADER_RFRAME_POS_FLAG = 7;
        public const byte UHFREADER_RFRAME_POS_CMD = 8;
        public const byte UHFREADER_RFRAME_POS_RFU = 9;
        public const byte UHFREADER_RFRAME_POS_PAR = 10;

        public const byte UHFREADER_FRAME_RESPONSE_FLAG = 0x1F;

        public const int UHFREADER_FRAME_MIN_LEN = 11;
        public const int UHFREADER_RFRAME_MIN_LEN = 14;
        public const int UHFREADER_FRAME_DATA_MAX_LEN = 240;
        public const int UHFREADER_FRAME_DATA_MIN_LEN = 4;

        public const byte UHFREADER_RFU = 0x00;

        public const int UHFREADER_BUFFER_MAX = 4096;
        public const int UHFREADER_VERSION_SIZE = 50;

        public const byte UHFREADER_CMD_RESET = 0x04;
        public const byte UHFREADER_CMD_GET_TAG_INFO = 0xF9;
        public const byte UHFREADER_CMD_GET_UHF_TMPR = 0xF8;
        public const byte UHFREADER_CMD_GET_VERSION = 0xF7;
        public const byte UHFREADER_CMD_GET_UHF_RTL = 0xF6;
        public const byte UHFREADER_CMD_GET_CFG = 0xF5;
        public const byte UHFREADER_CMD_SET_CFG = 0xF4;
        public const byte UHFREADER_CMD_GET_ANTCFG = 0xF1;
        public const byte UHFREADER_CMD_SET_ANTCFG = 0xF0;
        public const byte UHFREADER_CMD_INVT = 0x10;
        public const byte UHFREADER_CMD_USR_INVT = 0x11;
        public const byte UHFREADER_CMD_READ_BANK = 0x12;

        public const int UHFREADER_CMD_OPFRAME_TO = 500;

        public const Byte UHFREADER_ANT_NUM = 4;
        public const Byte UHFREADER_EPC_LEN = 32;

        public const Byte UHFREADER_WM_CMD = 0;
        public const Byte UHFREADER_WM_TIM = 1;

        public const Byte UHFREADER_CMD_MODE_HEX = 0;
        public const Byte UHFREADER_CMD_MODE_TXT = 1;

        public const Byte UHFREADER_BR_115200 = 0;
        public const Byte UHFREADER_BR_38400 = 1;
        public const Byte UHFREADER_BR_9600 = 2;

        public const Byte UHFREADER_SESSION_NUM = 4;
        public const Byte UHFREADER_TARGET_NUM = 2;
        public const Int32 UHFREADER_TAG_NUM = 256;
        public const Byte UHFREADER_MEM_WORD_LEN = 32;		//单位：字
        public const Byte UHFREADER_BANK_NUM = 4;
        public const Byte UHFREADER_PWD_LEN = 4;

        public const Byte UHFREADER_OP_RLT_OK = 0x00;
        public const Byte UHFREADER_OP_RLT_ERR = 0x01;
        public const Byte UHFREADER_OP_RLT_ERRTO = 0x02;
        public const Byte UHFREADER_OP_RLT_ERRLEN = 0x03;

        public const Byte UHFREADER_LOCK_TYPE_NUM = 4;

        public const Byte UHFREADER_MATCH_EPC_ENABLE = 0x00;
        public const Byte UHFREADER_MATCH_EPC_DISABLE = 0x01;

        //public static int hr2000Device;
        //public static int hr2000Device;
        ///<summary>
        /// API LoadLibrary
        ///</summary>
        [DllImport("Kernel32")]
        public static extern int LoadLibrary(String funcname);

        ///<summary>
        /// API GetProcAddress
        ///</summary>
        [DllImport("Kernel32")]
        public static extern int GetProcAddress(int handle, String funcname);

        ///<summary>
        /// API FreeLibrary
        ///</summary>
        [DllImport("Kernel32")]
        public static extern int FreeLibrary(int handle);

        ///<summary>
        ///通过非托管函数名转换为对应的委托, by jingzhongrong
        ///</summary>
        ///<param name="dllModule">通过LoadLibrary获得的DLL句柄</param>
        ///<param name="functionName">非托管函数名</param>
        ///<param name="t">对应的委托类型</param>
        ///<returns>委托实例，可强制转换为适当的委托类型</returns>
        public static Delegate GetFunctionAddress(int dllModule, string functionName, Type t)
        {
            int address = GetProcAddress(dllModule, functionName);
            if (address == 0)
            {
                return null;
            }
            else
            {
                return Marshal.GetDelegateForFunctionPointer(new IntPtr(address), t);
            }
        }

        ///<summary>
        ///将表示函数地址的IntPtr实例转换成对应的委托
        ///</summary>
        public static Delegate GetDelegateFromIntPtr(IntPtr address, Type t)
        {
            if (address == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                return Marshal.GetDelegateForFunctionPointer(address, t);
            }
        }

        ///<summary>
        ///将表示函数地址的int转换成对应的委托
        ///</summary>
        public static Delegate GetDelegateFromIntPtr(int address, Type t)
        {
            if (address == 0)
            {
                return null;
            }
            else
            {
                return Marshal.GetDelegateForFunctionPointer(new IntPtr(address), t);
            }
        }

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderOpenPort")]
        public static extern int uhfReaderOpenPort(String portName, String baudrate);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderClosePort")]
        public static extern int uhfReaderClosePort(int h);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderOpenUsb")]
        public static extern int uhfReaderOpenUsb(ushort vid, ushort pid);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderScanUsbList")]
        public static extern int uhfReaderScanUsbList(ushort vid, ushort pid, int[] handleList);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderCloseUsb")]
        public static extern int uhfReaderCloseUsb(int h);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderOpenSocket")]
        public static extern int uhfReaderOpenSocket(String ip, ushort port);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderCloseSocket")]
        public static extern int uhfReaderCloseSocket(int h);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderAnalyseFrame")]
        public static extern int uhfReaderAnalyseFrame(Byte byt, ref UHFREADER_RCVFRAME pRcvFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderFormatFrame")]
        public static extern int uhfReaderFormatFrame(ushort srcAddr, ushort targetAddr, Byte cmd, Byte rspFrame, Byte longFrame, Byte[] pParam, uint len, Byte[] pFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderReceiveFrame")]
        public static extern int uhfReaderReceiveFrame(int h, Byte[] pBuffer, int timeout);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderTransFrame")]
        public static extern int uhfReaderTransFrame(int h, Byte[] pFrame, uint len);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderReceiveBytes")]
        public static extern int uhfReaderReceiveBytes(int h, Byte[] pBuffer, int timeout);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderGetVersion")]
        public static extern int uhfReaderGetVersion(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_VERSION pVersion, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderGetTemperature")]
        public static extern int uhfReaderGetTemperature(int h, ushort srcAddr, ushort targetAddr, int[] pTempr, ref UHFREADER_OPRESULT pResult, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderGetReturnloss")]
        public static extern int uhfReaderGetReturnloss(int h, ushort srcAddr, ushort targetAddr, Byte ant, Byte frqPoint, int[] pReturnloss, ref UHFREADER_OPRESULT pResult, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderGetSysParams")]
        public static extern int uhfReaderGetSysParams(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_SYSPARAMS pSysParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderSetSysParams")]
        public static extern int uhfReaderSetSysParams(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_SYSPARAMS pSysParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderGetAntParams")]
        public static extern int uhfReaderGetAntParams(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_ANTPARAMS pAntParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderSetAntParams")]
        public static extern int uhfReaderSetAntParams(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_ANTPARAMS pAntParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderInventory")]
        public static extern int uhfReaderInventory(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_INVTPARAMS pInventoryParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderUsrInventory")]
        public static extern int uhfReaderUsrInventory(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_INVTPARAMS pInventoryParams, Byte session, Byte target, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderSetMask")]
        public static extern int uhfReaderSetMask(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_MASKPARAMS pMaskParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderClearMask")]
        public static extern int uhfReaderClearMask(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_OPRESULT pResult, Byte[] pTxFrame, Byte[] pRxFrame);


        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderReadMemory")]
        public static extern int uhfReaderReadMemory(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_RMEMPARAMS pMemoryParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderWriteMemory")]
        public static extern int uhfReaderWriteMemory(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_WMEMPARAMS pMemoryParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderLockBank")]
        public static extern int uhfReaderLockBank(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_LOCKPARAMS pLockParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderKillTag")]
        public static extern int uhfReaderKillTag(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_KILLPARAMS pKillParams, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderGetMatchEpc")]
        public static extern int uhfReaderGetMatchEpc(int h, ushort srcAddr, ushort targetAddr, ref UHFREADER_EPC pEpc, ref UHFREADER_OPRESULT pResult, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderSetMatchEpc")]
        public static extern int uhfReaderSetMatchEpc(int h, ushort srcAddr, ushort targetAddr, Byte mode, UHFREADER_EPC epc, ref UHFREADER_OPRESULT pResult, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderGetTagInfo")]
        public static extern int uhfReaderGetTagInfo(int h, ushort srcAddr, ushort targetAddr, Byte index, Byte[] pAnt, Byte[] pRssi, ref UHFREADER_OPRESULT pResult, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderSetHeartTick")]
        public static extern int uhfReaderSetHeartTick(int h, ushort srcAddr, ushort targetAddr, uint tick, ref UHFREADER_OPRESULT pResult, Byte[] pTxFrame, Byte[] pRxFrame);

        [DllImport("UHFReader.dll", EntryPoint = "uhfReaderSetW232Params")]
        public static extern int uhfReaderSetW232Params(int h, ushort srcAddr, ushort targetAddr, Byte[] w232Params, int paramsLen, Byte state, ref UHFREADER_OPRESULT pResult, Byte[] pTxFrame, Byte[] pRxFrame);
    }
}
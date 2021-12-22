using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Win32
{
    /// <summary>
    /// A static class that provides the win32 P/Invoke signatures 
    /// used by this class.
    /// </summary>
    /// <remarks>
    /// Note:  In each of the declarations below, we explicitly set CharSet to 
    /// Auto.  By default in C#, CharSet is set to Ansi, which reduces 
    /// performance on windows 2000 and above due to needing to convert strings
    /// from Unicode (the native format for all .Net strings) to Ansi before 
    /// marshalling.  Using Auto lets the marshaller select the Unicode version of 
    /// these functions when available.
    /// </remarks>
    [System.Security.SuppressUnmanagedCodeSecurity]
    public static class Win32Lib
    {

        #region User32

        //显示窗口
        public const int WS_SHOWNORMAL = 1;

        /// <summary>
        /// 设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="nCmdShow">指定窗口如何显示</param>
        /// <returns>如果窗口原来可见，返回值为非零；如果窗口原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// 创建指定窗口的线程设置到前台，并且激活该窗口
        /// </summary>
        /// <param name="hWnd">将要设置前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SetForegroundWindow(IntPtr hWnd);


        #region 系统热键
        //如果函数执行成功，返回值不为0。
        //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
          IntPtr hWnd,                  //要定义热键的窗口的句柄
          int id,                       //定义热键ID（不能与其它ID重复）      
          KeyModifiers fsModifiers,     //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
          System.Windows.Forms.Keys vk  //定义热键的内容
          );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
          IntPtr hWnd,      //要取消热键的窗口的句柄
          int id           //要取消热键的ID
          );

        //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }
        #endregion

        #endregion


        #region Kernel32

        /// <summary>
        ///  retrieves the names of all sections in an initialization file. 
        /// </summary>
        /// <param name="lpszReturnBuffer">指向一个缓冲，用来保存返回的所有section</param>
        /// <param name="nSize">lpszReturnBuffer的大小</param>
        /// <param name="lpFileName">初始化文件的名字。如没有指定完整路径名，windows就在Windows目录中查找文件</param>
        /// <returns>装载到lpszReturnBuffer缓冲区的字符数量</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer,
                                                               uint nSize,
                                                               string lpFileName);

        /// <summary>
        /// 获取初始化文件中指定的条目的字符
        /// </summary>
        /// <param name="lpAppName">欲在其中查找条目的小节名称</param>
        /// <param name="lpKeyName">欲获取的项名或条目名</param>
        /// <param name="lpDefault">指定的条目没有找到时返回的默认值</param>
        /// <param name="lpReturnedString">指定一个字串缓冲区，长度至少为nSize</param>
        /// <param name="nSize">指定装载到lpReturnedString缓冲区的最大字符数量</param>
        /// <param name="lpFileName">初始化文件的名字。如没有指定完整路径名，windows就在Windows目录中查找文件</param>
        /// <returns>复制到lpReturnedString缓冲区的字节数量</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetPrivateProfileString(string lpAppName,
                                                          string lpKeyName,
                                                          string lpDefault,
                                                          StringBuilder lpReturnedString,
                                                          int nSize,
                                                          string lpFileName);

        /// <summary>
        /// 获取初始化文件中指定的条目的字符
        /// </summary>
        /// <param name="lpAppName">欲在其中查找条目的小节名称</param>
        /// <param name="lpKeyName">欲获取的项名或条目名</param>
        /// <param name="lpDefault">指定的条目没有找到时返回的默认值</param>
        /// <param name="lpReturnedString">指定一个字串缓冲区，长度至少为nSize</param>
        /// <param name="nSize">指定装载到lpReturnedString缓冲区的最大字符数量</param>
        /// <param name="lpFileName">初始化文件的名字。如没有指定完整路径名，windows就在Windows目录中查找文件</param>
        /// <returns>复制到lpReturnedString缓冲区的字节数量</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetPrivateProfileString(string lpAppName,
                                                          string lpKeyName,
                                                          string lpDefault,
                                                          [In, Out] char[] lpReturnedString,
                                                          int nSize,
                                                          string lpFileName);

        /// <summary>
        /// 获取初始化文件中指定条目的字符
        /// </summary>
        /// <param name="lpAppName">欲在其中查找条目的小节名称</param>
        /// <param name="lpKeyName">欲获取的项名或条目名</param>
        /// <param name="lpDefault">指定的条目没有找到时返回的默认值</param>
        /// <param name="lpReturnedString">指定一个字串缓冲区，长度至少为nSize</param>
        /// <param name="nSize">指定装载到lpReturnedString缓冲区的最大字符数量</param>
        /// <param name="lpFileName">初始化文件的名字。如没有指定完整路径名，windows就在Windows目录中查找文件</param>
        /// <returns>复制到lpReturnedString缓冲区的字节数量</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileString(string lpAppName,
                                                         string lpKeyName,
                                                         string lpDefault,
                                                         IntPtr lpReturnedString,
                                                         uint nSize,
                                                         string lpFileName);

        /// <summary>
        /// 获取初始化文件中指定条目的整数值
        /// </summary>
        /// <param name="lpAppName">欲在其中查找条目的小节名称</param>
        /// <param name="lpKeyName">欲获取的项名或条目名</param>
        /// <param name="lpDefault">指定的条目没有找到时返回的默认值</param>
        /// <param name="lpFileName">初始化文件的名字。如没有指定完整路径名，windows就在Windows目录中查找文件</param>
        /// <returns>找到的条目的值</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileInt(string lpAppName,
                                                      string lpKeyName,
                                                      int lpDefault,
                                                      string lpFileName);

        /// <summary>
        /// 获取指定小节所有项名和值的一个列表
        /// </summary>
        /// <param name="lpAppName">欲获取的小节</param>
        /// <param name="lpReturnedString">缓冲区</param>
        /// <param name="nSize">lpReturnedString缓冲区的大小</param>
        /// <param name="lpFileName">初始化文件的名字。如没有指定完整路径名，windows就在Windows目录中查找文件</param>
        /// <returns>装载到lpReturnedString缓冲区的字符数量</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileSection(string lpAppName,
                                                          IntPtr lpReturnedString,
                                                          uint nSize,
                                                          string lpFileName);

        /// <summary>
        /// 在初始化文件指定小节内设置一个字串
        /// </summary>
        /// <param name="lpAppName">要在其中写入新字串的小节名称</param>
        /// <param name="lpKeyName">要设置的项名或条目名</param>
        /// <param name="lpString">指定为这个项写入的字串值</param>
        /// <param name="lpFileName">初始化文件的名字。如果没有指定完整路径名，则windows会在windows目录查找文件。如果文件没有找到，则函数会创建它</param>
        /// <returns>非零表示成功，零表示失败</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool WritePrivateProfileString(string lpAppName,
                                                            string lpKeyName,
                                                            string lpString,
                                                            string lpFileName);

        #endregion


        [DllImport("Iphlpapi.dll")]
        public static extern int SendARP(Int32 dest, Int32 host, byte[] mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        public static extern Int32 inet_addr(string ip);
    }
}

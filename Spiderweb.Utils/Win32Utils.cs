using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Spiderweb.Win32;

namespace Spiderweb.Utils
{
    public static class Win32Utils
    {
        /// <summary>
        /// 获取当前程序在进程中的对象
        /// </summary>
        /// <param name="curProcess">进程对象</param>
        /// <returns></returns>
        public static Process GetRunningInstance()
        {
            Process curProc = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curProc.ProcessName);

            //遍历相同名称的进程
            foreach (Process proc in procs)
            {
                //忽略现有的进程
                if (proc.Id == curProc.Id) continue;                

                //判断启动路径是否一致
                if (proc.MainModule.FileName.Equals(curProc.MainModule.FileName))
                {//Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == curProc.MainModule.FileName
                    return proc;
                }
            }

            //内存中不存在此当前程序的对象
            return null;
        }

        /// <summary>
        /// 切换进程至最前端
        /// </summary>
        /// <param name="proc">进程对象</param>
        public static void ShowRunningInstance(Process proc)
        {
            if (null == proc) return;
            //显示进程主窗口
            Win32Lib.ShowWindowAsync(proc.MainWindowHandle, Win32Lib.WS_SHOWNORMAL);
            //设置进程窗口到最前面
            Win32Lib.SetForegroundWindow(proc.MainWindowHandle);
        }


        /// <summary>
        /// 获取Ini文件指定参数的字符串
        /// </summary>
        /// <param name="filePath">ini文件路径</param>
        /// <param name="sectionName">ini文件条目</param>
        /// <param name="keyName">ini文件参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string ReadIniString(string filePath, string sectionName, string keyName, string defaultValue)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            StringBuilder retval = new StringBuilder(32767);//32KB

            Win32Lib.GetPrivateProfileString(sectionName, keyName, defaultValue, retval, retval.Capacity, filePath);

            return retval.ToString();
        }

        /// <summary>
        /// 获取Ini文件指定参数的整数值
        /// </summary>
        /// <param name="filePath">ini文件路径</param>
        /// <param name="sectionName">ini文件条目</param>
        /// <param name="keyName">ini文件参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int ReadIniInt(string filePath, string sectionName, string keyName, int defaultValue)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            return Win32Lib.GetPrivateProfileInt(sectionName, keyName, defaultValue, filePath);
        }

        /// <summary>
        /// 将值写入到指定参数的ini文件中
        /// </summary>
        /// <param name="filePath">ini文件路径</param>
        /// <param name="sectionName">ini文件条目</param>
        /// <param name="keyName">ini文件参数名</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool WriteIniString(string filePath, string sectionName, string keyName, string value)
        {
            bool retValue = Win32Lib.WritePrivateProfileString(sectionName, keyName, value, filePath);
            if (!retValue)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return retValue;
        }


        public static string ReadFromRegistry(RegistryKey regHive, string regPath, string keyName, string strDefault)
        {
            string result = "";

            string[] regStrings;
            regStrings = regPath.Split('\\');

            //First item of array will be the base key, so be carefull iterating below
            RegistryKey[] regKey = new RegistryKey[regStrings.Length + 1];
            regKey[0] = regHive;

            try
            {
                for (int i = 0; i < regStrings.Length; i++)
                {
                    regKey[i + 1] = regKey[i].OpenSubKey(regStrings[i]);

                    if (i == regStrings.Length - 1)
                    {
                        result = (string)regKey[i + 1].GetValue(keyName, strDefault);
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public static bool WriteToRegistry(RegistryKey regHive, string regPath, string keyName, string keyValue)
        {
            // Split the registry path 
            string[] regStrings;
            regStrings = regPath.Split('\\');

            // First item of array will be the base key, so be carefull iterating below
            RegistryKey[] regKey = new RegistryKey[regStrings.Length + 1];
            regKey[0] = regHive;

            for (int i = 0; i < regStrings.Length; i++)
            {
                regKey[i + 1] = regKey[i].OpenSubKey(regStrings[i], true);
                // If key does not exist, create it
                if (regKey[i + 1] == null)
                {
                    regKey[i + 1] = regKey[i].CreateSubKey(regStrings[i]);
                }
            }
            // Write the value to the registry
            try
            {
                regKey[regStrings.Length].SetValue(keyName, keyValue);
                return true;
            }
            catch (System.NullReferenceException ex1)
            {
                Console.WriteLine(ex1.Message);
                return false;

            }
            catch (System.UnauthorizedAccessException ex2)
            {
                Console.WriteLine(ex2.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

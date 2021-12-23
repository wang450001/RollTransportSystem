using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.Conf
{
    /**
     * Author: spiderweb
     * 
     * Date: 2019-10-30
     * 
     * Function: 配置工具类
     * 
     * */
    public abstract class CAppConfig
    {
        public static CAppConfig CreateInstance(string typeName, string pathOrConnection)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(pathOrConnection)) return null;

            return (CAppConfig)Activator.CreateInstance(Type.GetType(typeName), pathOrConnection);
        }

        /// <summary>
        /// 配置路径或连接字符串
        /// </summary>
        public string Path { get; private set; }

        protected CAppConfig(string path)
        {
            this.Path = path.ToString();
        }

        /// <summary>
        /// 读取指定参数的配置值
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">字段名</param>
        /// <param name="strDefault">默认值</param>
        /// <returns></returns>
        public abstract string Read(string section, string key, string strDefault = "");

        /// <summary>
        /// 读取指定参数的配置值
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">字段名</param>
        /// <param name="intDefault">默认值</param>
        /// <returns></returns>
        public abstract int Read(string section, string key, int intDefault = 0);

        /// <summary>
        /// 给指定参数写入值
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">字段名</param>
        /// <param name="strValue">写入值</param>
        /// <returns></returns>
        public abstract bool Write(string section, string key, string strValue);


        static class CAppConfigConst
        {
            static string config_namespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;//"com.spiderweb.CommonLibs.config.";
            const string CONFIG_TYPE_INI = "IniAppConfig";
            const string CONFIG_TYPE_XML = "XmlAppConfig";
            const string CONFIG_TYPE_DB = "DbAppConfig";
            const string CONFIG_TYPE_REG = "RegAppConfig";

            static Dictionary<string, string> dict;

            static CAppConfigConst()
            {
                dict = new Dictionary<string, string>();
                dict.Add(CONFIG_TYPE_INI, config_namespace + "." + CONFIG_TYPE_INI);
                dict.Add(CONFIG_TYPE_XML, config_namespace + "." + CONFIG_TYPE_XML);
                dict.Add(CONFIG_TYPE_DB, config_namespace + "." + CONFIG_TYPE_DB);
                dict.Add(CONFIG_TYPE_REG, config_namespace + "." + CONFIG_TYPE_REG);
            }

            public static string GetQualifiedClassName(string clsName)
            {
                if (dict == null) return string.Empty;

                if (dict.ContainsKey(clsName))
                    return dict[clsName];

                return string.Empty;
            }
        }
    }
}

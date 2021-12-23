using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Spiderweb.Common.Conf
{
    public class AppSettingConfig : CAppConfig
    {
        public AppSettingConfig(string path) : base(path) { }

        public override string Read(string section, string key, string strDefault = "")
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch { return strDefault; }
        }

        public override int Read(string section, string key, int intDefault = 0)
        {
            try
            {
                int.TryParse(ConfigurationManager.AppSettings[key].ToString(), out intDefault);
                return intDefault;
            }
            catch { return intDefault; }
        }

        public override bool Write(string section, string key, string strValue)
        {
            return false;
        }
    }
}

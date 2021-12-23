using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spiderweb.Utils;

namespace Spiderweb.Common.Conf
{
    public class RegAppConfig : CAppConfig
    {
        RegistryKey regHive;

        public RegAppConfig(string path)
            : base("Software\\Spiderweb\\{0}")
        {
            regHive = Microsoft.Win32.Registry.LocalMachine;
        }

        public override string Read(string section, string key, string strDefault = "")
        {
            //string regPath = string.Format("Software\\Spiderweb\\{0}", section);
            string regPath = string.Format(Path, section);
            return Win32Utils.ReadFromRegistry(regHive, regPath, key, strDefault);
        }

        public override int Read(string section, string key, int intDefault = 0)
        {
            int intTemp = 0;

            if (!int.TryParse(Read(section, key, "0"), out intTemp))
                intTemp = intDefault;

            return intTemp;
        }

        public override bool Write(string section, string key, string strValue)
        {
            //string regPath = string.Format("Software\\Spiderweb\\{0}", section);
            string regPath = string.Format(Path, section);
            return Win32Utils.WriteToRegistry(regHive, regPath, key, strValue);
        }
    }
}

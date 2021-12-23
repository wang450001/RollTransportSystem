using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spiderweb.Utils;

namespace Spiderweb.Common.Conf
{
    public class IniAppConfig : CAppConfig
    {
        public IniAppConfig(string path) : base(path) { }

        public override string Read(string section, string key, string strDefault = "")
        {
            return Win32Utils.ReadIniString(Path, section, key, strDefault);
        }

        public override int Read(string section, string key, int intDefault = 0)
        {
            return Win32Utils.ReadIniInt(Path, section, key, intDefault);
        }

        public override bool Write(string section, string key, string strValue)
        {
            return Win32Utils.WriteIniString(Path, section, key, strValue);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.Conf
{
    public class DbAppConfig : CAppConfig
    {
        public DbAppConfig(string connString)
            : base(connString)
        {

        }

        public override string Read(string section, string key, string strDefault = "")
        {
            throw new NotImplementedException();
        }

        public override int Read(string section, string key, int intDefault = 0)
        {
            throw new NotImplementedException();
        }

        public override bool Write(string section, string key, string strValue)
        {
            throw new NotImplementedException();
        }
    }
}

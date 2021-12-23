using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.Security
{
    /* *
     *      对称加密算法DES
     *      
     * 功能：字符串或文本文件的加解密
     * 
     * 作者：Spiderweb
     * 
     * 日期：2020-3-3
     * 
     * 
     * */
    public class DESProvider : CSecurityProvider
    {
        public DESProvider(string key, string iv) : base(key, iv) { }

        protected override void Init()
        {
            Algorithm = new DESCryptoServiceProvider();

            base.Init();
        }
    }
}

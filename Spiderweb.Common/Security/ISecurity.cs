using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.Security
{
    public interface ISecurity
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plaintext">原始字符串</param>
        /// <returns></returns>
        string Encrypt(string plaintext);

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="plaintext">明文文件</param>
        /// <param name="ciphertext">加密后的密文</param>
        /// <returns></returns>
        bool Encrypt(string plaintext, string ciphertext);


        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        string Decrypt(string ciphertext);


        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="ciphertext">密文文件</param>
        /// <param name="plaintext">解密后的明文</param>
        /// <returns></returns>
        bool Decrypt(string ciphertext, string plaintext);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.Security
{
    /* *
     *      非对称加密算法MD5
     *      
     * 功能：字符串或文本文件的加解密
     * 
     * 作者：Spiderweb
     * 
     * 日期：2020-3-3
     * 
     * 
     * */
    public class MD5Provider : CSecurityProvider
    {
        private MD5 md5;

        public MD5Provider() : this(null, null) { }

        public MD5Provider(string key, string iv) : base(key, iv) { }

        protected override void Init()
        {
            md5 = new MD5CryptoServiceProvider();
        }

        /// <summary>
        /// 使用32位加密字符
        /// </summary>
        /// <param name="plaintext">待加密字符</param>
        /// <returns></returns>
        public override string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext)) return null;

            byte[] bPlaintext = UTF8Encoding.UTF8.GetBytes(plaintext);

            byte[] bCiphertext = md5.ComputeHash(bPlaintext);

            string strReturn = "";
            //for (int i = 0; i < bCiphertext.Length; i++)
            //    strReturn += Convert.ToString(bCiphertext[i], 16).PadLeft(2, '0');
            
            strReturn = BitConverter.ToString(bCiphertext).Replace("-", "");

            return strReturn.PadLeft(32, '0').ToUpper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public override bool Encrypt(string originalFile, string newFile)
        {
            if (!File.Exists(originalFile)) return false;

            string fileName = CreatePath(originalFile, newFile);

            //加密
            FileStream fin = null, fout = null;

            try
            {
                fin = new FileStream(originalFile, FileMode.Open, FileAccess.Read);
                fout = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

                byte[] bOriginal = new byte[fin.Length];
                fin.Read(bOriginal, 0, bOriginal.Length);

                byte[] bCipher = md5.ComputeHash(bOriginal);
               
                string result = BitConverter.ToString(bCipher);

                byte[] bResult = Encoding.UTF8.GetBytes(result.Replace("-", ""));

                //fout.Write(bOriginal, 0, bOriginal.Length);
                fout.Write(bResult, 0, bResult.Length);

                fout.Flush();
            }
            catch { return false; }
            finally
            {
                //关闭数据流
                if (fout != null) fout.Close();
                if (fin != null) fin.Close();
            }

            return true;
        }


        public override string Decrypt(string ciphertext)
        {
            return null;
        }

        public override bool Decrypt(string ciphertext, string plaintext)
        {
            return false;
        }
    }
}

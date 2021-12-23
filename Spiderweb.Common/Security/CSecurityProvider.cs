using Spiderweb.Common.log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Common.Security
{
    public abstract class CSecurityProvider : ISecurity
    {
        private const string ALG_KEY = "ovenking_scut";
        private const string ALG_IV = "728#$$%^TyguyshdsufhsfwofnhKJHJKHIYhfiusf98*(^%$^&&(*&()$##@%%$RHGJJHHJ";
        protected string Key;
        protected string IV;
        protected int UnitLength;
        protected SymmetricAlgorithm Algorithm;

        public CLogger Logger { get; set; }

        protected CSecurityProvider(string key, string iv)
        {
            UnitLength = 100;

            if (string.IsNullOrEmpty(key))
                this.Key = ALG_KEY;
            else
                this.Key = key;

            if (string.IsNullOrEmpty(iv))
                this.IV = ALG_IV;
            else
                this.IV = iv;

            Init();
        }

        protected virtual void Init() 
        {
            if (Algorithm == null) return;
            Algorithm.Key = GetKey();
            Algorithm.IV = GetIV();
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        private byte[] GetKey()
        {
            Algorithm.GenerateKey();
            return GetBytes(Key, Algorithm.Key.Count());
        }

        /// <summary>
        /// 获取初始向量IV
        /// </summary>
        /// <returns></returns>
        private byte[] GetIV()
        {
            Algorithm.GenerateIV();
            return GetBytes(IV, Algorithm.IV.Count());
        }

        /// <summary>
        /// 获取指定长度BYTE数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private byte[] GetBytes(string str, int len)
        {
            byte[] byteArray = new byte[len];

            if (str.Length < len)
                //长度不足则补空
                byteArray = ASCIIEncoding.ASCII.GetBytes(str.PadRight(len, ' '));
            else
                //长度过长则截取
                byteArray = ASCIIEncoding.ASCII.GetBytes(str.Substring(0, len));

            return byteArray;
        }

        public static CSecurityProvider CreateInstance(string clsName)
        {
            return CreateInstance(clsName, null, null);
        }

        public static CSecurityProvider CreateInstance(string typeName, object key, object iv)
        {
            if (string.IsNullOrEmpty(typeName)) return null;

            return (CSecurityProvider)Activator.CreateInstance(Type.GetType(typeName), key, iv);
        }

        /// <summary>
        /// 加密字符
        /// </summary>
        /// <param name="plaintext">原始的文本文件</param>
        /// <returns>加密成功返回TRUE，失败返回FALSE</returns>
        public virtual string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext)) return null;

            byte[] bPlaintext = UTF8Encoding.UTF8.GetBytes(plaintext);

            MemoryStream ms = new MemoryStream();
            ICryptoTransform transform = Algorithm.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);

            //将明文加密后写入MemoryStream
            cs.Write(bPlaintext, 0, bPlaintext.Length);
            cs.Close();
            ms.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="plaintext">原始的文本文件</param>
        /// <param name="ciphertext">加密后的文件</param>
        /// <returns>加密成功返回TRUE，失败返回FALSE</returns>
        public virtual bool Encrypt(string plaintext, string ciphertext)
        {
            ICryptoTransform transform = Algorithm.CreateEncryptor();
            return CryptoAlgorithm(plaintext, ciphertext, transform);
        }

        public virtual string Decrypt(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext)) return null;

            byte[] bCiphertext = Convert.FromBase64String(ciphertext);

            MemoryStream ms = new MemoryStream(bCiphertext, 0, bCiphertext.Length);
            ICryptoTransform transform = Algorithm.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Read);

            StreamReader sr = new StreamReader(cs);
            string plaintext = null;
            try
            {
                plaintext = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (null != Logger)                
                    Logger.Log(MessageType.Error, string.Format("字符<{0}>解密出错，原因：{1}\r\n{2}。", ciphertext, ex.Source, ex.StackTrace)); 
            }
            finally
            {
                sr.Close();
                cs.Close();
                ms.Close();
            }           

            return plaintext;
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="ciphertext">加密的文件</param>
        /// <param name="plaintext">解密的文件</param>
        /// <returns>解密成功返回TRUE，失败返回FALSE</returns>
        public virtual bool Decrypt(string ciphertext, string plaintext)
        {
            ICryptoTransform transform = Algorithm.CreateDecryptor();
            return CryptoAlgorithm(ciphertext, plaintext, transform);
        }

        protected string CreatePath(string originalFile, string newFile)
        {            
            //获取加密文件的目录信息
            string dirPath, fileName;
            if (Spiderweb.Utils.CommonUtils.IsDir(newFile))
            {
                dirPath = newFile;
                fileName = string.Format("{0}\\{1}.des", dirPath, originalFile.Substring(originalFile.LastIndexOf('\\') + 1));
            }
            else
            {
                dirPath = newFile.Substring(0, newFile.LastIndexOf('\\'));
                fileName = newFile;
            }

            //目录不存在则创建
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            return fileName;
        }

        protected bool CryptoAlgorithm(string originalFile, string newFile, ICryptoTransform transform)
        {
            //判断原始文件是否存在
            if (!File.Exists(originalFile)) return false;

            string fileName = CreatePath(originalFile, newFile);

            //解密
            FileStream fin = null, fout = null;
            CryptoStream cs = null;
            byte[] bOriginalData = new byte[UnitLength];
            int readLen;

            try
            {
                fin = new FileStream(originalFile, FileMode.Open, FileAccess.Read);
                fout = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                long totalLen = fin.Length, index = 0;

                cs = new CryptoStream(fout, transform, CryptoStreamMode.Write);

                while (index < totalLen)
                {
                    readLen = fin.Read(bOriginalData, 0, bOriginalData.Length);
                    cs.Write(bOriginalData, 0, readLen);
                    index += readLen;
                }
                fout.Flush();
            }
            catch(Exception ex)
            {
                if (null != Logger)
                    Logger.Log(MessageType.Error, string.Format("文件<{0}>加密失败，原因：{1}\r\n{2}", originalFile, ex.Source, ex.StackTrace));
                
                return false;
            }
            finally
            {
                //关闭数据流
                if (cs != null) cs.Close();
                if (fout != null) fout.Close();
                if (fin != null) fin.Close();
            }

            return true;
        }
    }
}

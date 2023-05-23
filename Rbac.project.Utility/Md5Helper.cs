using System;
using System.Security.Cryptography;
using System.Text;

namespace Rbac.project.Utility
{
    public static class Md5Helper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = md5.ComputeHash(sourceBytes);
            string hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashedString;
        }

        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5(this string input)
        {
            MD5 md5 = MD5.Create();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = md5.ComputeHash(sourceBytes);
            string hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashedString;
        }
    }
}

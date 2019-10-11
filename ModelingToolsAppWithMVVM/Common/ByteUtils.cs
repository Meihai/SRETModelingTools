using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{  
    /// <summary>
    /// 数组工具类
    /// </summary>
    public class ByteUtils
    {
        /// <summary>
        /// string类型转成byte[]
        /// </summary>
        /// <param name="convertedStr"></param>
        /// <returns></returns>
        public static byte[] StringToByteArr(string convertedStr)
        {
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(convertedStr);
            return byteArray;
        }

        /// <summary>
        /// 数组转字符串
        /// </summary>
        /// <param name="byteArr"></param>
        /// <returns></returns>
        public static string ByteArrToString(byte[] byteArr)
        {
            string str = System.Text.Encoding.Default.GetString(byteArr);
            return str;
        }

        /// <summary>
        /// string 类型转ASCII byte[]
        /// </summary>
        /// <param name="convertedStr"></param>
        /// <returns></returns>
        public static byte[] StringToASCIIByteArr(string convertedStr) {
            //"01" 转成 byte[] = new byte[]{ 0x30,0x31}
            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(convertedStr);
            return byteArray;
        }


        /// <summary>
        /// ASCIIbyte[]转成string
        /// </summary>
        /// <param name="byteArr"></param>
        /// <returns></returns>
        public static string ASCIIByteArrToString(byte[] byteArr)
        {
            //byte[] = new byte[]{ 0x30, 0x31} 转成"01"

            string str = System.Text.Encoding.ASCII.GetString(byteArr);
            return str;
        }

        /// <summary>
        /// byte[]转16进制格式string：
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            //new byte[]{ 0x30, 0x31}转成"3031":
            string hexString = string.Empty;
            if (bytes != null)
            {
                System.Text.StringBuilder strB = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 16进制格式string 转byte[]：
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] hexStringToBytes(string hexString)
        {
            if (hexString == null || hexString.Equals(""))
            {
                return null;
            }
            hexString = hexString.ToUpper();
            int length = hexString.Length / 2;
            char[] hexChars = hexString.ToCharArray();
            byte[] d = new byte[length];
            for (int i = 0; i < length; i++)
            {
                int pos = i * 2;
                d[i] = (byte)(Convert.ToByte(hexChars[pos]) << 4 | Convert.ToByte(hexChars[pos + 1]));
            }
            return d;
        }


    }
}

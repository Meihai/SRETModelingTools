using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 16Bit CRC 校验类
    /// </summary>
    public class CrcUtils
    {
        /// <summary>
        /// 生成16位CRC校验码
        /// </summary>
        /// <param name="bufData"></param>
        /// <param name="buflen"></param>
        /// <returns></returns>
        public static byte[] Get_crc16(byte[] bufData, int buflen)
        {
            byte[] pcrc = new byte[2];
            int CRC = 0x0000ffff;
            int POLYNOMIAL = 0x0000a001;
            int i, j;
            for (i = 0; i < buflen; i++)
            {
                CRC ^= ((int)bufData[i] & 0x000000ff);
                for (j = 0; j < 8; j++)
                {
                    if ((CRC & 0x00000001) != 0)
                    {
                        CRC >>= 1;
                        CRC ^= POLYNOMIAL;
                    }
                    else {
                        CRC >>= 1;
                    }
                }
            }
           // Console.WriteLine(CRC.ToString("x"));
            pcrc[0] = (byte)(CRC & 0x00ff);
            pcrc[1] = (byte)(CRC >> 8);
            return pcrc;
        }

        /// <summary>
        /// 验证16位CRC校验码
        /// </summary>
        /// <param name="bufData"></param>
        /// <returns></returns>
        public static bool Valid_crc16(byte[] bufData)
        {
            byte[] pcrc = new byte[2];
            int CRC = 0x0000ffff;
            int POLYNOMIAL = 0x0000a001;
            int i, j;
            for (i = 0; i < bufData.Length - 2; i++)
            {
                CRC ^= ((int)bufData[i] & 0x000000fff);
                for (j = 0; j < 8; j++)
                {
                    if ((CRC & 0x00000001) != 0)
                    {
                        CRC >>= 1;
                        CRC ^= POLYNOMIAL;
                    }
                    else
                    {
                        CRC >>= 1;
                    }
                }
            }
            pcrc[0] = (byte)(CRC & 0x00ff);
            pcrc[1] = (byte)(CRC >> 8);
            if (pcrc[0] == bufData[bufData.Length - 2] && pcrc[1] == bufData[bufData.Length - 1])
            {
                return true;
            }
            return false;
        }
    }
}

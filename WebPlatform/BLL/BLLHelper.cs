using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebPlatform.BLL
{
    public class BLLHelper
    {
        public static string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x2");

            }
            return pwd;
        }


        /// <summary>
        /// 计算CRC-16
        /// </summary>
        /// <param name="data"></param>
        /// <returns>高位在前</returns>
        //public static string CRC_16(string data)
        //{
        //    if ((data.Trim().Length % 2) != 0)
        //    {
        //        throw new Exception("参数\"data\"长度不合法");
        //    }

        //    byte[] tmp = StrToByte(data.Trim());

        //    /*
        //    1、预置16位寄存器为十六进制FFFF（即全为1）。称此寄存器为CRC寄存器； 
        //    2、把第一个8位数据与16位CRC寄存器的低位相异或，把结果放于CRC寄存器； 
        //    3、把寄存器的内容右移一位(朝低位)，用0填补最高位，检查最低位； 
        //    4、如果最低位为0：重复第3步(再次移位); 如果最低位为1：CRC寄存器与多项式A001（1010 0000 0000 0001）进行异或； 
        //    5、重复步骤3和4，直到右移8次，这样整个8位数据全部进行了处理； 
        //    6、重复步骤2到步骤5，进行下一个8位数据的处理； 
        //    7、最后得到的CRC寄存器即为CRC码。
        //    */
        //    UInt16 CRCREG = (UInt16)0xffff;
        //    for (int i = 0; i < tmp.Length; i++)
        //    {
        //        CRCREG = (UInt16)(CRCREG ^ (UInt16)tmp[i]);//<< 8;
        //        for (int j = 0; j < 8; j++)
        //        {
        //            UInt16 CRCtmp = (UInt16)(CRCREG & (UInt16)0x0001);
        //            CRCREG = (UInt16)(CRCREG >> (UInt16)1);
        //            if (CRCtmp == (UInt16)1)
        //            {
        //                CRCREG = (UInt16)(CRCREG ^ (UInt16)0xA001);
        //            }
        //        }
        //    }

        //    string strtmp = CRCREG.ToString("X4");
        //    byte[] retunBtye = new byte[8];
        //    tmp.CopyTo(retunBtye, 0);
        //    retunBtye[6] = StrToByte(strtmp.Substring(2, 2))[0];
        //    retunBtye[7] = StrToByte(strtmp.Substring(0, 2))[0];

        //    string retStr = string.Empty;

        //    for (int i = 0; i < retunBtye.Length; i++)
        //    {
        //        retStr += retunBtye[i].ToString("X2") + " ";
        //    }

        //    return retStr;
        //}

        #region 计算CRC校验码
        /// <summary>
        /// 计算CRC校验码，并转换为十六进制字符串
        /// Cyclic Redundancy Check 循环冗余校验码
        /// 是数据通信领域中最常用的一种差错校验码
        /// 特征是信息字段和校验字段的长度可以任意选定
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CRC_16(string input)
        {
            byte[] data = StrToByte(input.Replace(" ", ""));

            byte num = 0xff;
            byte num2 = 0xff;

            byte num3 = 1;
            byte num4 = 160;
            byte[] buffer = data;

            for (int i = 0; i < buffer.Length; i++)
            {
                //位异或运算
                num = (byte)(num ^ buffer[i]);

                for (int j = 0; j <= 7; j++)
                {
                    byte num5 = num2;
                    byte num6 = num;

                    //位右移运算
                    num2 = (byte)(num2 >> 1);
                    num = (byte)(num >> 1);

                    //位与运算
                    if ((num5 & 1) == 1)
                    {
                        //位或运算
                        num = (byte)(num | 0x80);
                    }
                    if ((num6 & 1) == 1)
                    {
                        num2 = (byte)(num2 ^ num4);
                        num = (byte)(num ^ num3);
                    }
                }
            }
            string retStr = string.Empty;
            //return byteToHexStr(new byte[] { num, num2 }, 2);
            for (int i = 0; i < data.Length; i++)
            {
                retStr += data[i].ToString("X2") + " ";
            }

            retStr += num.ToString("X2") + " ";
            retStr += num2.ToString("X2");

            return retStr;
        }

        #endregion

        private static byte[] StrToByte(string data)
        {
            byte[] bt = new byte[data.Length / 2];
            for (int i = 0; i < data.Length / 2; i++)
            {
                bt[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            }
            return bt;
        }
    }
}
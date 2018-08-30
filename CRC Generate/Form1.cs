using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRC_Generate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                int addrDec = int.Parse(txtAddr.Text);
                for (int i = 0; i < numCount.Value; i++)
                {
                    //MessageBox.Show((addrDec + i).ToString("X2"));
                    string addrHex = (addrDec + i).ToString("X2");
                    txtList.Text += CRC_16(addrHex + txtFunCode.Text.Trim() + txtData.Text.Trim()) + "\r\n";
                }

                MessageBox.Show("Generate ok.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            try
            {
                string[] strSrc = this.txtInput.Text.Trim().Split("\r\n".ToCharArray());
                txtList.Text = string.Empty;

                for (int i = 0; i < strSrc.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strSrc[i].Trim()))
                    {
                        txtList.Text += CRC_16(strSrc[i].Trim()) + "\r\n";
                    }
                }

                MessageBox.Show("Generate ok.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 计算CRC-16
        /// </summary>
        /// <param name="data"></param>
        /// <returns>高位在前</returns>
        public string CRC_16(string data)
        {
            if ((data.Trim().Length % 2) != 0)
            {
                throw new Exception("参数\"data\"长度不合法");
            }

            byte[] tmp = StrToByte(data.Replace(" ", ""));

            /*
            1、预置16位寄存器为十六进制FFFF（即全为1）。称此寄存器为CRC寄存器； 
            2、把第一个8位数据与16位CRC寄存器的低位相异或，把结果放于CRC寄存器； 
            3、把寄存器的内容右移一位(朝低位)，用0填补最高位，检查最低位； 
            4、如果最低位为0：重复第3步(再次移位); 如果最低位为1：CRC寄存器与多项式A001（1010 0000 0000 0001）进行异或； 
            5、重复步骤3和4，直到右移8次，这样整个8位数据全部进行了处理； 
            6、重复步骤2到步骤5，进行下一个8位数据的处理； 
            7、最后得到的CRC寄存器即为CRC码。
            */
            UInt16 CRCREG = (UInt16)0xffff;
            for (int i = 0; i < tmp.Length; i++)
            {
                CRCREG = (UInt16)(CRCREG ^ (UInt16)tmp[i]);//<< 8;
                for (int j = 0; j < 8; j++)
                {
                    UInt16 CRCtmp = (UInt16)(CRCREG & (UInt16)0x0001);
                    CRCREG = (UInt16)(CRCREG >> (UInt16)1);
                    if (CRCtmp == (UInt16)1)
                    {
                        CRCREG = (UInt16)(CRCREG ^ (UInt16)0xA001);
                    }
                }
            }

            string strtmp = CRCREG.ToString("X4");
            byte[] retunBtye = new byte[8];
            tmp.CopyTo(retunBtye, 0);
            retunBtye[6] = StrToByte(strtmp.Substring(2, 2))[0];
            retunBtye[7] = StrToByte(strtmp.Substring(0, 2))[0];

            string retStr = string.Empty;

            for (int i = 0; i < retunBtye.Length; i++)
            {
                retStr += retunBtye[i].ToString("X2") + " ";
            }

            return retStr;
        }

        public byte[] StrToByte(string data)
        {
            byte[] bt = new byte[data.Length / 2];
            for (int i = 0; i < data.Length / 2; i++)
            {
                bt[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            }
            return bt;
        }

        #region 计算CRC校验码
        /// <summary>
        /// 计算CRC校验码，并转换为十六进制字符串
        /// Cyclic Redundancy Check 循环冗余校验码
        /// 是数据通信领域中最常用的一种差错校验码
        /// 特征是信息字段和校验字段的长度可以任意选定
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string CRC16_C(string input)
        {
            byte[] data = StrToByte(input.Replace(" ",""));

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

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string byteToHexStr(byte[] bytes, int size)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < size; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }


        private void btnCrc32_Click(object sender, EventArgs e)
        {
            MessageBox.Show(CRC16_C(this.txtInput.Text));
        }
    }
}

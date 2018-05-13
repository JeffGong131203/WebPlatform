using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace SerialPortSvc
{
    /// <summary>
    /// SerialPort 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SerialPortData : System.Web.Services.WebService
    {
        private static SerialPort sp = new SerialPort();
        private static string _reciveData = string.Empty;
        private static string _sendData = string.Empty;
        private static string _deviceID = string.Empty;
        private static DateTime _sendTime;

        [WebMethod]
        public bool PortStatus()
        {
            return sp.IsOpen;
        }

        [WebMethod]
        public bool OpenPort()
        {
            List<string> comList = GetComlist(false); //首先获取本机关联的串行端口列表            
            if (comList.Count == 0)
            {
                throw new Exception("当前设备不存在串行端口！");
                //System.Environment.Exit(0); //彻底退出应用程序   
            }
            else
            {
                string targetCOMPort = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();

                //判断串口列表中是否存在目标串行端口
                if (!comList.Contains(targetCOMPort))
                {
                    throw new Exception("当前设备不存在配置的串行端口！");
                    //System.Environment.Exit(0); //彻底退出应用程序   
                }

                //sp = new SerialPort();
                sp.PortName = targetCOMPort;

                sp.BaudRate = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["BaudRate"].ToString()); //串行波特率
                //sp.BaudRate = 115200;
                //sp.DataBits = 8; //每个字节的标准数据位长度
                sp.DataBits = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DataBits"].ToString());
                //sp.StopBits = StopBits.One; //设置每个字节的标准停止位数
                sp.StopBits = (StopBits)Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["StopBits"].ToString());
                //sp.Parity = Parity.None; //设置奇偶校验检查协议
                sp.Parity = (Parity)Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Parity"].ToString());
                //sp.ReadTimeout = 3000; //单位毫秒
                sp.ReadTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["ReadTimeout"].ToString());
                //sp.WriteTimeout = 3000; //单位毫秒
                sp.WriteTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["WriteTimeout"].ToString());
                //串口控件成员变量，字面意思为接收字节阀值，
                //串口对象在收到这样长度的数据之后会触发事件处理函数
                //一般都设为1
                //sp.ReceivedBytesThreshold = 1;
                sp.ReceivedBytesThreshold = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["ReceivedBytesThreshold"].ToString());

                sp.DataReceived += new SerialDataReceivedEventHandler(CommDataReceived); //设置数据接收事件（监听）

                try
                {
                    if (!sp.IsOpen)
                    {
                        sp.Open(); //打开串口

                        return true;

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("串行端口打开失败！具体原因：" + ex.Message);
                    //System.Environment.Exit(0); //彻底退出应用程序   

                }

                return false;
            }


        }

        [WebMethod]
        public bool ClosePort()
        {
            try
            {
                sp.Close();

                return true;

            }
            catch
            {
                return false;
            }

        }

        [WebMethod]
        public bool SendData(string data,string deviceID)
        {
            try
            {
                byte[] hexSendData = HexStringToByteArray(data);

                if (sp != null)
                {
                    _reciveData = string.Empty;
                    sp.Write(hexSendData, 0, hexSendData.Length);
                    _sendData = data;
                    _deviceID = deviceID;
                    _sendTime = DateTime.Now;

                    return true;
                }
                else
                {
                    throw new Exception("当前设备串行端口未打开！");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Thread.Sleep(3000);

        }

        [WebMethod]
        public string GetReciveData()
        {
            return _reciveData;
        }

        /// <summary>
        /// 串口数据处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommDataReceived(Object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //Comm.BytesToRead中为要读入的字节长度
                int byteNumber = sp.BytesToRead;
                Thread.Sleep(20);

                //延时等待数据接收完毕。
                while ((byteNumber < sp.BytesToRead) && (sp.BytesToRead < 4800))
                {
                    byteNumber = sp.BytesToRead;
                    Thread.Sleep(20);
                }

                int n = sp.BytesToRead; //记录下缓冲区的字节个数 
                byte[] buf = new byte[n]; //声明一个临时数组存储当前来的串口数据 
                sp.Read(buf, 0, n); //读取缓冲数据到buf中，同时将这串数据从缓冲区移除 

                //处理readBuffer中的数据，自定义处理过程
                string msg = ByteArrayToHexString(buf);  //获取出入库产品编号

                ProcessData(msg);
            }
            catch (Exception ex)
            {
                throw new Exception("接收返回消息异常！具体原因：" + ex.Message);
            }
        }

        private void ProcessData(string strData)
        {
            _reciveData = string.Empty;

            Dictionary<string, string> retData = new Dictionary<string, string>();
            retData.Add("DeviceID", _deviceID);
            retData.Add("SendTime", _sendTime.ToString());
            retData.Add("SendData", _sendData);
            retData.Add("ReciveTime", DateTime.Now.ToString());
            retData.Add("ReciveData", strData);

            _reciveData = JsonConvert.SerializeObject(retData, Formatting.Indented); 
        }

        private static byte[] HexStringToByteArray(string s)
        {
            try
            {
                if (s.Length == 0)
                {
                    throw new Exception("将16进制字符串转换成字节数组时出错，错误信息：被转换的字符串长度为0。");
                }

                s = s.Replace(" ", "");
                byte[] buffer = new byte[s.Length / 2];

                for (int i = 0; i < s.Length; i += 2)
                {
                    buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
                }

                return buffer;
            }
            catch (Exception ex)
            {
                throw new Exception("将16进制字符串转换成字节数组时出错！具体原因：" + ex.Message);
            }
        }

        private static string ByteArrayToHexString(byte[] ReceivedData)
        {
            String RecvDataText = string.Empty;
            for (int i = 0; i < ReceivedData.Length - 1; i++)
            {
                RecvDataText += ("0x" + ReceivedData[i].ToString("X2") + "");
            }

            return RecvDataText;
        }

        /// <summary>
        /// 获取本机串口列表
        /// </summary>
        /// <param name="isUseReg"></param>
        /// <returns></returns>
        private List<string> GetComlist(bool isUseReg)
        {
            List<string> list = new List<string>();
            try
            {
                if (isUseReg)
                {
                    RegistryKey RootKey = Registry.LocalMachine;
                    RegistryKey Comkey = RootKey.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM");

                    String[] ComNames = Comkey.GetValueNames();

                    foreach (String ComNamekey in ComNames)
                    {
                        string TemS = Comkey.GetValue(ComNamekey).ToString();
                        list.Add(TemS);
                    }
                }
                else
                {
                    foreach (string com in SerialPort.GetPortNames())  //自动获取串行口名称  
                        list.Add(com);
                }
            }
            catch
            {
                throw new Exception("串行端口检查异常！");
                //System.Environment.Exit(0); //彻底退出应用程序   
            }
            return list;
        }

    }
}

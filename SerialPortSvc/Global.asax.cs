using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SerialPortSvc
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                string portName = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();

                WriteLog(portName,"SvcStart",DateTime.Now.ToString());

                double intval = double.Parse(System.Configuration.ConfigurationManager.AppSettings["SvcIntval"].ToString());

                Timer t = new Timer(intval);
                t.Elapsed += T_Elapsed;
                t.Start();
            }
            catch(Exception ex)
            {
                string portName = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();

                WriteLog(portName, "SvcError", ex.Message);

            }

        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                DataTable dt = GetSendData();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SendData(dr["DeviceID"].ToString(), dr["SendData"].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                string portName = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();

                WriteLog(portName, "TimerError", ex.Message);
            }
        }

        private DataTable GetSendData()
        {
            DataTable dt = new DataTable();

            try
            {
                string portName = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();
                string sql = string.Format("select t1.DeviceID,t1.SendData from Device_Send t1 inner join Device_Info t2 on t1.DeviceID = t2.ID and t2.ParentID = (select id from Device_Info where DevName='{0}')", portName);

                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebPlatform"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
            }
            catch(Exception ex)
            {
                string portName = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();

                WriteLog(portName,"GetSendDataError",ex.Message);
            }

            return dt;
        }

        private void SendData(string deviceID,string sendData)
        {
            SerialPortSvc.SerialPortData spd = new SerialPortData();

            try
            {
                if(!spd.PortStatus())
                {
                    spd.OpenPort();
                }

                spd.SendData(sendData, deviceID);

                string reciveData = spd.GetReciveData();

                DateTime t1 = DateTime.Now;
                DateTime t2 = DateTime.Now;

                while(string.IsNullOrEmpty(reciveData))
                {
                    TimeSpan ts = t2 - t1;

                    reciveData = spd.GetReciveData();

                    if(ts.TotalSeconds>5)
                    {
                        break;
                    }

                    t2 = DateTime.Now;
                }
                
                if(string.IsNullOrEmpty(reciveData))
                {
                    string portName = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();

                    WriteLog(portName,"SendDataTimeout",deviceID+":"+sendData);
                }
                else
                {
                    WriteData(reciveData);
                }
            }
            catch(Exception ex)
            {
                string portName = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();

                WriteLog(portName, "SendDataError", ex.Message);
            }
        }

        private void WriteData(string reciveData)
        {
            try
            {
                Dictionary<string, string> dicRecData = JsonConvert.DeserializeObject<Dictionary<string, string>>(reciveData);

                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebPlatfromReciveData"].ConnectionString))
                {
                    conn.Open();
                    string sql = string.Format("insert into ReciveData values('{0}','{1}','{2}','{3}','{4}','{5}')",Guid.NewGuid(),dicRecData["DeviceID"],dicRecData["SendTime"], dicRecData["SendData"], dicRecData["ReciveTime"], dicRecData["ReciveData"]);
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                string portName = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();

                WriteLog(portName, "WriteDataError", ex.Message);

            }
        }
        

        private void WriteLog(string portName,string msgType,string logMsg)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebPlatfromReciveData"].ConnectionString))
                {
                    conn.Open();

                    string sql = string.Format("insert into PortLog Values('{0}','{1}','{2}','{3}','{4}')",Guid.NewGuid(),DateTime.Now,msgType,logMsg,portName);

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                }
            }
            catch(Exception ex)
            {

            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WebPlatform;

namespace SfcCinemaDataSvc
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                System.Timers.Timer t = new System.Timers.Timer();
                t.Interval = 600000;//10 min run
                t.Enabled = true;

                t.Elapsed += new System.Timers.ElapsedEventHandler(T_Elapsed);
                t.Start();

                WriteLog("Service Start...");
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //string url = "http://report.sfccinema.com/wx/showPlans";
            string Formaturl = "http://report.sfccinema.com/wx/showPlans?cinemaID={0}";
            Dictionary<string, string> dicParm = new Dictionary<string, string>();
            string[] cinemaID = System.Configuration.ConfigurationManager.AppSettings["cinemaID"].Split(";".ToCharArray());

            foreach (string cid in cinemaID)
            {
                string url = string.Format(Formaturl,cid);

                string retData = WebPlatform.BLL.HTTPHelper.HttpPostResponse(url, dicParm);
                DataTable dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(retData);

                dt.TableName = "SfcCinemaData";

                if (dt.Rows.Count > 0)
                {
                    string startDate = dt.Rows[0]["startDate"].ToString();

                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebPlatformDataContext"].ConnectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(string.Format("delete from SfcCinemaData where startDate='{0}'", startDate), conn);

                        cmd.ExecuteNonQuery();

                        using (SqlBulkCopy sqlBC = new SqlBulkCopy(conn))
                        {
                            //一次批量的插入的数据量
                            sqlBC.BatchSize = 1000;
                            //超时之前操作完成所允许的秒数，如果超时则事务不会提交 ，数据将回滚，所有已复制的行都会从目标表中移除
                            sqlBC.BulkCopyTimeout = 600;

                            //設定 NotifyAfter 属性，以便在每插入10000 条数据时，呼叫相应事件。  
                            sqlBC.NotifyAfter = 10000;
                            sqlBC.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);

                            //设置要批量写入的表
                            sqlBC.DestinationTableName = dt.TableName;

                            //自定义的datatable和数据库的字段进行对应
                            //sqlBC.ColumnMappings.Add("id", "tel");
                            //sqlBC.ColumnMappings.Add("name", "neirong");

                            //批量写入
                            sqlBC.WriteToServer(dt);
                        }
                    }
                }
            }
        }

        private static void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            //MyLog.WriteLog(string.Format("{0} rows has instered", e.RowsCopied));
        }

        private void WriteLog(string msg)
        {
            string deptt = "log_" + DateTime.Now.ToString() + ".txt";
            string logPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + deptt;

            try
            {
                using (StreamWriter sw = new StreamWriter(logPath, true, Encoding.UTF8))
                {
                    sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + msg);
                }
            }
            catch
            {
            }


        }

        protected override void OnStop()
        {
            WriteLog("Service Stop...");

        }
    }
}

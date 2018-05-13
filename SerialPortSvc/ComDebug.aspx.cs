using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SerialPortSvc
{
    public partial class ComDebug : System.Web.UI.Page
    {
        private SerialPortSvc.SerialPortData spd = new SerialPortData();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblComName.Text = System.Configuration.ConfigurationManager.AppSettings["COMPort"].ToString();

            lblStatus.Text = spd.PortStatus().ToString();

        }

        protected void btnOpen_Click(object sender, EventArgs e)
        {
            spd.OpenPort();

            lblStatus.Text = spd.PortStatus().ToString();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            spd.ClosePort();

            lblStatus.Text = spd.PortStatus().ToString();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            spd.SendData(txtSendData.Text, "DebugDevice");

            //DateTime t1 = DateTime.Now;
            //DateTime t2 = DateTime.Now;

            //TimeSpan ts = t2 - t1;

            //while (!string.IsNullOrEmpty(spd.GetReciveData()) || ts.TotalSeconds > 3)
            //{
            //    t2 = DateTime.Now;

                txtRecive.Text += spd.GetReciveData() + "\r\n";
            //}
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            txtRecive.Text += spd.GetReciveData() + "\r\n";
        }
    }
}
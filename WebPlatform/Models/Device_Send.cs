using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebPlatform.Models
{
    public class Device_Send
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("设备ID")]
        public Nullable<System.Guid> DeviceID { get; set; }
        [DisplayName("发送指令")]
        public string SendData { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebPlatform.Models
{
    public class Device_Data
    {
        [DisplayName("ID")]
        public Guid ID { get; set; }
        [DisplayName("设备ID")]
        public Guid DeviceID { get; set; }
        [DisplayName("指令发送时间")]
        public DateTime SendTime { get; set; }
        [DisplayName("指令字符串")]
        public string SendData { get; set; }
        [DisplayName("数据返回时间")]
        public DateTime ReciveTime { get; set; }
        [DisplayName("返回数据")]
        public string ReciveData { get; set; }
    }
}
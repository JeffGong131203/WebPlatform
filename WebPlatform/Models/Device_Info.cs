using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebPlatform.Models
{
    public class Device_Info
    {
        [DisplayName("ID")]
        public Guid ID { get; set; }
        [DisplayName("设备编号")]
        public string DevCode { get; set; }
        [DisplayName("设备名称")]
        public string DevName { get; set; }
        [DisplayName("上级设备ID")]
        public Guid ParentID { get; set; }
        [DisplayName("设备类型")]
        public int DevType { get; set; }
        [DisplayName("设备属性")]
        public string PropertyJson { get; set; }
    }
}
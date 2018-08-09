using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebPlatform.Models
{
    public class Device_Customer
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("客户ID")]
        public Nullable<System.Guid> CustomerID { get; set; }
        [DisplayName("设备ID")]
        public Nullable<System.Guid> DeviceID { get; set; }
    }
}
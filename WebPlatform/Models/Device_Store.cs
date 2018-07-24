using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebPlatform.Models
{
    public class Device_Store
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("设备ID")]
        public Nullable<System.Guid> DeviceID { get; set; }
        [DisplayName("门店ID")]
        public Nullable<System.Guid> StoreID { get; set; }
    }
}
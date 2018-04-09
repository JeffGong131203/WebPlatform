using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Customer_Area
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("客户ID")]
        public Nullable<System.Guid> CusID { get; set; }
        [DisplayName("大区编号")]
        public string AreaCode { get; set; }
        [DisplayName("大区名称")]
        public string AreaName { get; set; }
    }
}

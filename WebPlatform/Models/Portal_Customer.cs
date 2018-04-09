using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Customer
    {
        [DisplayName("客户ID")]
        public System.Guid ID { get; set; }
        [DisplayName("客户编号")]
        public string CusCode { get; set; }
        [DisplayName("客户名称")]
        public string CusName { get; set; }
    }
}

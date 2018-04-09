using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Customer_Store
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("大区ID")]
        public Nullable<System.Guid> AreaID { get; set; }
        [DisplayName("门店编号")]
        public string StoreCode { get; set; }
        [DisplayName("门店名称")]
        public string StoreName { get; set; }
    }
}

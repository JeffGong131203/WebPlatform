using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_User_Customer
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("用户")]
        public Nullable<System.Guid> UserID { get; set; }
        [DisplayName("客户")]
        public Nullable<System.Guid> CusID { get; set; }
        [DisplayName("大区")]
        public Nullable<System.Guid> AreaID { get; set; }
        [DisplayName("门店")]
        public Nullable<System.Guid> StoreID { get; set; }
    }
}

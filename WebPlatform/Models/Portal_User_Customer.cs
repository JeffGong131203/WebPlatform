using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_User_Customer
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("�û�")]
        public Nullable<System.Guid> UserID { get; set; }
        [DisplayName("�ͻ�")]
        public Nullable<System.Guid> CusID { get; set; }
        [DisplayName("����")]
        public Nullable<System.Guid> AreaID { get; set; }
        [DisplayName("�ŵ�")]
        public Nullable<System.Guid> StoreID { get; set; }
    }
}

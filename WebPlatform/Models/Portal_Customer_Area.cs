using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Customer_Area
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("�ͻ�ID")]
        public Nullable<System.Guid> CusID { get; set; }
        [DisplayName("�������")]
        public string AreaCode { get; set; }
        [DisplayName("��������")]
        public string AreaName { get; set; }
    }
}

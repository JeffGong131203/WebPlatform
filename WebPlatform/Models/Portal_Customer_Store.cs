using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Customer_Store
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("����ID")]
        public Nullable<System.Guid> AreaID { get; set; }
        [DisplayName("�ŵ���")]
        public string StoreCode { get; set; }
        [DisplayName("�ŵ�����")]
        public string StoreName { get; set; }
    }
}

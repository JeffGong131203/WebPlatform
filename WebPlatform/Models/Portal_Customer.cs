using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Customer
    {
        [DisplayName("�ͻ�ID")]
        public System.Guid ID { get; set; }
        [DisplayName("�ͻ����")]
        public string CusCode { get; set; }
        [DisplayName("�ͻ�����")]
        public string CusName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Auth
    {
        [DisplayName("Ȩ��ID")]
        public System.Guid ID { get; set; }
        [DisplayName("Ȩ�ޱ��")]
        public string AuthCode { get; set; }
        [DisplayName("Ȩ������")]
        public string AuthName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_User
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("��¼�ʺ�")]
        public string Loginno { get; set; }
        [DisplayName("����")]
        public string Loginpsw { get; set; }
        [DisplayName("�Ƿ�����")]
        public Nullable<bool> IsUsed { get; set; }
    }
}

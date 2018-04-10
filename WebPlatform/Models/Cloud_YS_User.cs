using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Cloud_YS_User
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("�û�ID")]
        public Nullable<System.Guid> UserID { get; set; }
        [DisplayName("өʯ���˺�ID")]
        public string YsAccount { get; set; }
        [DisplayName("өʯ���˺�����")]
        public string YsPsw { get; set; }
        [DisplayName("өʯAppKey")]
        public string YsAppKey { get; set; }
        [DisplayName("өʯSecret")]
        public string YsSecret { get; set; }
    }
}

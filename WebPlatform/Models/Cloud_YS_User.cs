using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Cloud_YS_User
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("用户ID")]
        public Nullable<System.Guid> UserID { get; set; }
        [DisplayName("萤石子账号ID")]
        public string YsAccount { get; set; }
        [DisplayName("萤石子账号密码")]
        public string YsPsw { get; set; }
        [DisplayName("萤石AppKey")]
        public string YsAppKey { get; set; }
        [DisplayName("萤石Secret")]
        public string YsSecret { get; set; }
    }
}

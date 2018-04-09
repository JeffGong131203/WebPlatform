using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Auth_User
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("权限ID")]
        public Nullable<System.Guid> AuthID { get; set; }
        [DisplayName("用户ID")]
        public Nullable<System.Guid> UserID { get; set; }
    }
}

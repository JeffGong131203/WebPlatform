using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Auth_Menu
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("È¨ÏÞID")]
        public Nullable<System.Guid> AuthID { get; set; }
        [DisplayName("²Ëµ¥ID")]
        public Nullable<System.Guid> MenuID { get; set; }
    }
}

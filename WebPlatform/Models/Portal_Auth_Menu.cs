using System;
using System.Collections.Generic;

namespace WebPlatform.Models
{
    public partial class Portal_Auth_Menu
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> AuthID { get; set; }
        public Nullable<System.Guid> MenuID { get; set; }
    }
}

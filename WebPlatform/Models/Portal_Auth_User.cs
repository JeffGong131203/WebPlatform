using System;
using System.Collections.Generic;

namespace WebPlatform.Models
{
    public partial class Portal_Auth_User
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> AuthID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
    }
}

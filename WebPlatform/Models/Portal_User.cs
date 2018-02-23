using System;
using System.Collections.Generic;

namespace WebPlatform.Models
{
    public partial class Portal_User
    {
        public System.Guid ID { get; set; }
        public string Loginno { get; set; }
        public string Loginpsw { get; set; }
        public Nullable<bool> IsUsed { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace WebPlatform.Models
{
    public partial class Cloud_YS_User
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string YsAccount { get; set; }
        public string YsPsw { get; set; }
        public string YsAppKey { get; set; }
        public string YsSecret { get; set; }
    }
}

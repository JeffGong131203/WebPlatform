using System;
using System.Collections.Generic;

namespace WebPlatform.Models
{
    public partial class Portal_Menu
    {
        public System.Guid ID { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public Nullable<System.Guid> ParentID { get; set; }
    }
}

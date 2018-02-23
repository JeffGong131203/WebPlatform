using System;
using System.Collections.Generic;

namespace WebPlatform.Models
{
    public partial class Portal_Customer_Area
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CusID { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
    }
}

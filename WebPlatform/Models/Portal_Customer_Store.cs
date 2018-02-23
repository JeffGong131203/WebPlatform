using System;
using System.Collections.Generic;

namespace WebPlatform.Models
{
    public partial class Portal_Customer_Store
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> AreaID { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
    }
}

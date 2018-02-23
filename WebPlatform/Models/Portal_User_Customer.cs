using System;
using System.Collections.Generic;

namespace WebPlatform.Models
{
    public partial class Portal_User_Customer
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<System.Guid> CusID { get; set; }
        public Nullable<System.Guid> AreaID { get; set; }
        public Nullable<System.Guid> StoreID { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace WebPlatform.Models
{
    public partial class Portal_Auth
    {
        public System.Guid ID { get; set; }
        public string AuthCode { get; set; }
        public string AuthName { get; set; }
    }
}

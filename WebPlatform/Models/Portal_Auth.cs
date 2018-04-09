using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_Auth
    {
        [DisplayName("权限ID")]
        public System.Guid ID { get; set; }
        [DisplayName("权限编号")]
        public string AuthCode { get; set; }
        [DisplayName("权限名称")]
        public string AuthName { get; set; }
    }
}

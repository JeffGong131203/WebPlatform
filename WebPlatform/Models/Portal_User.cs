using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebPlatform.Models
{
    public partial class Portal_User
    {
        [DisplayName("ID")]
        public System.Guid ID { get; set; }
        [DisplayName("µ«¬º’ ∫≈")]
        public string Loginno { get; set; }
        [DisplayName("√‹¬Î")]
        public string Loginpsw { get; set; }
        [DisplayName(" «∑Ò∆Ù”√")]
        public Nullable<bool> IsUsed { get; set; }
    }
}

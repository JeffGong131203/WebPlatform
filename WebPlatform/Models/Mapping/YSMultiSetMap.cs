using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace WebPlatform.Models.Mapping
{
    public class YSMultiSetMap : EntityTypeConfiguration<YSMultiSet>
    {
        public YSMultiSetMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Table & Column Mappings
            this.ToTable(" YSMultiSet");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.YSMultiSetJson).HasColumnName("YSMultiSetJson");
        }
    }
}
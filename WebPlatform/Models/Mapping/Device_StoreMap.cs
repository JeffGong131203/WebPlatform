using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace WebPlatform.Models.Mapping
{
    public class Device_StoreMap : EntityTypeConfiguration<Device_Store>
    {
        public Device_StoreMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Table & Column Mappings
            this.ToTable("Device_Store");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.StoreID).HasColumnName("StoreID");
            this.Property(t => t.DeviceID).HasColumnName("DeviceID");
        }
    }
}
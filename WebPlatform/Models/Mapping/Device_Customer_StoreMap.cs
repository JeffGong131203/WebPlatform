using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace WebPlatform.Models.Mapping
{
    public class Device_Customer_StoreMap : EntityTypeConfiguration<Device_Customer_Store>
    {
        public Device_Customer_StoreMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Device_Customer_Store");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DeviceID).HasColumnName("DeviceID");
            this.Property(t => t.CusID).HasColumnName("CusID");
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            this.Property(t => t.StoreID).HasColumnName("StoreID");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace WebPlatform.Models.Mapping
{
    public class Device_SendMap : EntityTypeConfiguration<Device_Send>
    {
        public Device_SendMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Table & Column Mappings
            this.ToTable("Device_Send");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DeviceID).HasColumnName("DeviceID");
            this.Property(t => t.SendData).HasColumnName("SendData");
        }
    }
}
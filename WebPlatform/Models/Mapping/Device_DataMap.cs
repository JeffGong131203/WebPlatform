using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace WebPlatform.Models.Mapping
{
    public class Device_DataMap : EntityTypeConfiguration<Device_Data>
    {
        public Device_DataMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Table & Column Mappings
            this.ToTable("ReciveData");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DeviceID).HasColumnName("DeviceID");
            this.Property(t => t.SendTime).HasColumnName("SendTime");
            this.Property(t => t.SendData).HasColumnName("SendData");
            this.Property(t => t.ReciveTime).HasColumnName("ReciveTime");
            this.Property(t => t.ReciveData).HasColumnName("ReciveData");
        }
    }
}
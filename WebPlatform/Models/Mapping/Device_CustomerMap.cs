using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Device_CustomerMap : EntityTypeConfiguration<Device_Customer>
    {
        public Device_CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Table & Column Mappings
            this.ToTable("Device_Customer");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CustomerID).HasColumnName("CustomerID");
            this.Property(t => t.DeviceID).HasColumnName("DeviceID");
        }
    }
}
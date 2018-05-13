using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Device_UserMap : EntityTypeConfiguration<Device_User>
    {
        public Device_UserMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Table & Column Mappings
            this.ToTable("Device_User");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.DeviceID).HasColumnName("DeviceID");
        }
    }
}
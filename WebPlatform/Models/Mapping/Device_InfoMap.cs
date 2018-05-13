using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Device_InfoMap: EntityTypeConfiguration<Device_Info>
    {
        public Device_InfoMap()
        {
            this.HasKey(t => t.ID);

            this.Property(t => t.DevCode).HasMaxLength(50);
            this.Property(t => t.DevName).HasMaxLength(50);
            this.Property(t => t.DevType).HasMaxLength(50);
            this.Property(t => t.PropertyJson).HasMaxLength(4000);

            this.ToTable("Device_Info");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DevCode).HasColumnName("DevCode");
            this.Property(t => t.DevName).HasColumnName("DevName");
            this.Property(t => t.ParentID).HasColumnName("ParentID");
            this.Property(t => t.DevType).HasColumnName("DevType");
            this.Property(t => t.PropertyJson).HasColumnName("PropertyJson");
        }
    }
}
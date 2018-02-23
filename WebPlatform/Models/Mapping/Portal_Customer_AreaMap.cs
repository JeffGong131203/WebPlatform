using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Portal_Customer_AreaMap : EntityTypeConfiguration<Portal_Customer_Area>
    {
        public Portal_Customer_AreaMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.AreaCode)
                .HasMaxLength(50);

            this.Property(t => t.AreaName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Portal_Customer_Area");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CusID).HasColumnName("CusID");
            this.Property(t => t.AreaCode).HasColumnName("AreaCode");
            this.Property(t => t.AreaName).HasColumnName("AreaName");
        }
    }
}

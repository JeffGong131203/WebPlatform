using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Portal_CustomerMap : EntityTypeConfiguration<Portal_Customer>
    {
        public Portal_CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.CusCode)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Portal_Customer");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CusCode).HasColumnName("CusCode");
            this.Property(t => t.CusName).HasColumnName("CusName");
        }
    }
}

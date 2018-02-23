using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Portal_AuthMap : EntityTypeConfiguration<Portal_Auth>
    {
        public Portal_AuthMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.AuthCode)
                .HasMaxLength(50);

            this.Property(t => t.AuthName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Portal_Auth");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AuthCode).HasColumnName("AuthCode");
            this.Property(t => t.AuthName).HasColumnName("AuthName");
        }
    }
}

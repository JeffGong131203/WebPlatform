using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Portal_UserMap : EntityTypeConfiguration<Portal_User>
    {
        public Portal_UserMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Loginno)
                .HasMaxLength(50);

            this.Property(t => t.Loginpsw)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Portal_User");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Loginno).HasColumnName("Loginno");
            this.Property(t => t.Loginpsw).HasColumnName("Loginpsw");
            this.Property(t => t.IsUsed).HasColumnName("IsUsed");
        }
    }
}

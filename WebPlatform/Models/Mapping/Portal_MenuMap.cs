using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Portal_MenuMap : EntityTypeConfiguration<Portal_Menu>
    {
        public Portal_MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.MenuCode)
                .HasMaxLength(50);

            this.Property(t => t.MenuName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Portal_Menu");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.MenuCode).HasColumnName("MenuCode");
            this.Property(t => t.MenuName).HasColumnName("MenuName");
            this.Property(t => t.ParentID).HasColumnName("ParentID");
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Portal_Auth_MenuMap : EntityTypeConfiguration<Portal_Auth_Menu>
    {
        public Portal_Auth_MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Portal_Auth_Menu");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AuthID).HasColumnName("AuthID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
        }
    }
}

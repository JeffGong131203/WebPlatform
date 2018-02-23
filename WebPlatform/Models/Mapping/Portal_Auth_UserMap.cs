using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Portal_Auth_UserMap : EntityTypeConfiguration<Portal_Auth_User>
    {
        public Portal_Auth_UserMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Portal_Auth_User");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AuthID).HasColumnName("AuthID");
            this.Property(t => t.UserID).HasColumnName("UserID");
        }
    }
}

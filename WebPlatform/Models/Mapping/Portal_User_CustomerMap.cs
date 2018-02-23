using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Portal_User_CustomerMap : EntityTypeConfiguration<Portal_User_Customer>
    {
        public Portal_User_CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Portal_User_Customer");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.CusID).HasColumnName("CusID");
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            this.Property(t => t.StoreID).HasColumnName("StoreID");
        }
    }
}

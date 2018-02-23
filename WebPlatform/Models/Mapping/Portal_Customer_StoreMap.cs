using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Portal_Customer_StoreMap : EntityTypeConfiguration<Portal_Customer_Store>
    {
        public Portal_Customer_StoreMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.StoreCode)
                .HasMaxLength(50);

            this.Property(t => t.StoreName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Portal_Customer_Store");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            this.Property(t => t.StoreCode).HasColumnName("StoreCode");
            this.Property(t => t.StoreName).HasColumnName("StoreName");
        }
    }
}

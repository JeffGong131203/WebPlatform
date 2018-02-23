using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WebPlatform.Models.Mapping
{
    public class Cloud_YS_UserMap : EntityTypeConfiguration<Cloud_YS_User>
    {
        public Cloud_YS_UserMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.YsAccount)
                .HasMaxLength(50);

            this.Property(t => t.YsPsw)
                .HasMaxLength(50);

            this.Property(t => t.YsAppKey)
                .HasMaxLength(255);

            this.Property(t => t.YsSecret)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Cloud_YS_User");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.YsAccount).HasColumnName("YsAccount");
            this.Property(t => t.YsPsw).HasColumnName("YsPsw");
            this.Property(t => t.YsAppKey).HasColumnName("YsAppKey");
            this.Property(t => t.YsSecret).HasColumnName("YsSecret");
        }
    }
}

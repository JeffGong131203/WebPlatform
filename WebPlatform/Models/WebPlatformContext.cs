using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebPlatform.Models.Mapping;

namespace WebPlatform.Models
{
    public partial class WebPlatformContext : DbContext
    {
        static WebPlatformContext()
        {
            Database.SetInitializer<WebPlatformContext>(null);
        }

        public WebPlatformContext()
            : base("Name=WebPlatformContext")
        {
        }

        public DbSet<Cloud_YS_User> Cloud_YS_User { get; set; }
        public DbSet<Portal_Auth> Portal_Auth { get; set; }
        public DbSet<Portal_Auth_Menu> Portal_Auth_Menu { get; set; }
        public DbSet<Portal_Auth_User> Portal_Auth_User { get; set; }
        public DbSet<Portal_Customer> Portal_Customer { get; set; }
        public DbSet<Portal_Customer_Area> Portal_Customer_Area { get; set; }
        public DbSet<Portal_Customer_Store> Portal_Customer_Store { get; set; }
        public DbSet<Portal_Menu> Portal_Menu { get; set; }
        public DbSet<Portal_User> Portal_User { get; set; }
        public DbSet<Portal_User_Customer> Portal_User_Customer { get; set; }
        public DbSet<Device_Info> Device_Info { get; set; }
        public DbSet<Device_User> Device_User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Cloud_YS_UserMap());
            modelBuilder.Configurations.Add(new Portal_AuthMap());
            modelBuilder.Configurations.Add(new Portal_Auth_MenuMap());
            modelBuilder.Configurations.Add(new Portal_Auth_UserMap());
            modelBuilder.Configurations.Add(new Portal_CustomerMap());
            modelBuilder.Configurations.Add(new Portal_Customer_AreaMap());
            modelBuilder.Configurations.Add(new Portal_Customer_StoreMap());
            modelBuilder.Configurations.Add(new Portal_MenuMap());
            modelBuilder.Configurations.Add(new Portal_UserMap());
            modelBuilder.Configurations.Add(new Portal_User_CustomerMap());
            modelBuilder.Configurations.Add(new Device_InfoMap());
            modelBuilder.Configurations.Add(new Device_UserMap());
        }
    }
}

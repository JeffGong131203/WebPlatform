using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebPlatform.Models.Mapping;

namespace WebPlatform.Models
{
    public partial class WebPlatformDataContext : DbContext
    {
        static WebPlatformDataContext()
        {
            Database.SetInitializer<WebPlatformDataContext>(null);
        }

        public WebPlatformDataContext()
            : base("Name=WebPlatformDataContext")
        {
        }

        public DbSet<Device_Data> Device_Data { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Device_DataMap());
        }
    }
}
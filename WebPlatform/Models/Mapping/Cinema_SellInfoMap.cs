using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace WebPlatform.Models.Mapping
{
    public class Cinema_SellInfoMap : EntityTypeConfiguration<Cinema_SellInfo>
    {
        public Cinema_SellInfoMap()
        {
            this.HasKey(t => t.CinemaID);
            this.HasKey(t => t.HallID);
            this.HasKey(t => t.StartDate);
            this.HasKey(t => t.StartTime);
            this.HasKey(t => t.EndTime);

            this.Property(t => t.CinemaID).HasMaxLength(50);
            this.Property(t => t.HallID).HasMaxLength(50);
            this.Property(t => t.HallName).HasMaxLength(50);

            this.ToTable("SfcCinemaData");
            this.Property(t => t.CinemaID).HasColumnName("CinemaID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.SellCount).HasColumnName("SellCount");
            this.Property(t => t.SeatCount).HasColumnName("seatTotalNum");
        }
    }
}
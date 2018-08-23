using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebPlatform.Models
{
    public class Cinema_SellInfo
    {
        [DisplayName("影厅编号")]
        public string HallID { get; set; }
        [DisplayName("影院ID")]
        public string CinemaID { get; set; }
        [DisplayName("票房")]
        public int SellCount { get; set; }
        [DisplayName("座位总数")]
        public int SeatCount { get; set; }
        [DisplayName("开始时间")]
        public DateTime StartTime { get; set; }
        [DisplayName("结束时间")]
        public DateTime EndTime { get; set; }
        [DisplayName("影厅名称")]
        public string HallName { get; set; }
        [DisplayName("日期")]
        public DateTime StartDate { get; set; }
    }
}
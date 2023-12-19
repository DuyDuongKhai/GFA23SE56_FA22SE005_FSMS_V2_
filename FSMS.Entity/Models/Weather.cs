using System;
using System.Collections.Generic;

namespace FSMS.Entity.Models
{
    public partial class Weather
    {
        public int WeatherId { get; set; }
        public string? WeatherName { get; set; }
        public int UserId { get; set; }
        public string? Location { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}

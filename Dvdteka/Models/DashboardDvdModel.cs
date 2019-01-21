using System;

namespace Dvdteka.Models
{
    public class DashboardDvdModel
    {
        public string DvdName { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? TimesRented { get; set; }
    }
}

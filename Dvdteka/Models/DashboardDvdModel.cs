using System;

namespace Dvdteka.Models
{
    public class DashboardDvdModel
    {
        public int DvdId { get; set; }

        public string DvdName { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? TimesRented { get; set; }
    }
}

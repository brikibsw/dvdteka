using System;
using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Models
{
    public class RentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Dvd")]
        public string DvdName { get; set; }

        public int MemberId { get; set; }

        [Display(Name = "Član")]
        public string MemberName { get; set; }

        [Display(Name = "Datum posudbe")]
        public DateTime RentTime { get; set; }

        [Display(Name = "Datum povratka")]
        public DateTime? ReturnTime { get; set; }

        [Display(Name = "Cijena")]
        public decimal? Price { get; set; }

        public bool Returning { get; set; }

        public int DaysRented { get; set; }
    }
}

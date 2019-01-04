using System;
using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Data
{
    public class Rent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Dvd je obavezan podatak!")]
        [Display(Name = "Dvd")]
        public int DvdId { get; set; }

        public Dvd Dvd { get; set; }

        [Required(ErrorMessage = "Član je obavezan podatak!")]
        [Display(Name = "Član")]
        public int MemberId { get; set; }

        public Member Member { get; set; }

        [Required(ErrorMessage = "Vrijeme posudbe je obavezan podatak!")]
        [Display(Name = "Vrijeme posudbe")]
        public DateTime RentTime { get; set; }

        [Display(Name = "Vrijeme povratka")]
        public DateTime? ReturnTime { get; set; }

        [Display(Name = "Cijena")]
        public decimal? Price { get; set; }
    }
}

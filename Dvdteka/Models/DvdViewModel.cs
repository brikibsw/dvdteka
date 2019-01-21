using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dvdteka.Models
{
    public class DvdViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan podatak!")]
        [Display(Name = "Naziv")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Cijena je obavezan podatak!")]
        [Display(Name = "Cijena")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Godina je obavezan podatak!")]
        [Display(Name = "Godina")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Količina je obavezan podatak!")]
        [Display(Name = "Količina")]
        public int Quantity { get; set; }

        [Display(Name = "Dostupna količina")]
        public int AvailableQuantity { get; set; }

        [Required(ErrorMessage = "Režiser je obavezan podatak!")]
        [Display(Name = "Režiser")]
        public int DirectorId { get; set; }

        public string DirectorName { get; set; }

        [Required(ErrorMessage = "Žanr je obavezan podatak!")]
        [Display(Name = "Žanr")]
        public int GenreId { get; set; }

        public string GenreName { get; set; }

        public List<RentViewModel> OpenedRents { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Data
{
    public class Dvd
    {
        public Dvd()
        {
            Rents = new HashSet<Rent>();
        }

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

        public Director Director { get; set; }

        [Required(ErrorMessage = "Žanr je obavezan podatak!")]
        [Display(Name = "Žanr")]
        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public ICollection<Rent> Rents { get; set; }
    }
}

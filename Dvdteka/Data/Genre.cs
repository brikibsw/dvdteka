using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Data
{
    public class Genre
    {
        public Genre()
        {
            Dvds = new HashSet<Dvd>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan podatak!")]
        [Display(Name = "Naziv")]
        public string Name { get; set; }

        public ICollection<Dvd> Dvds { get; set; }
    }
}

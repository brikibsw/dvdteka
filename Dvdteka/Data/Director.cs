using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Data
{
    public class Director
    {
        public Director()
        {
            Dvds = new HashSet<Dvd>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime i prezime je obavezan podatak!")]
        [Display(Name = "Ime i prezime")]
        public string Name { get; set; }

        public ICollection<Dvd> Dvds { get; set; }
    }
}

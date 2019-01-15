using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Models
{
    public class ReturnManyViewModel
    {
        public List<RentViewModel> Rents { get; set; }

        [Display(Name = "Datum povratka")]
        [Required(ErrorMessage = "Vrijeme povratka je obavezno!")]
        public DateTime ReturnTime { get; set; }
    }
}

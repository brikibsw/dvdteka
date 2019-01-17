using System;
using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Data
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        public int DvdId { get; set; }

        public Dvd Dvd { get; set; }

        [Display(Name = "Dvd")]
        public string DvdName { get; set; }

        [Display(Name = "Vrijeme posudbe")]
        public DateTime RentTime { get; set; }

        [Display(Name = "Vrijeme povratka")]
        public DateTime ReturnTime { get; set; }

        [Display(Name = "Cijena")]
        public decimal Price { get; set; }
    }
}

using System;

namespace Dvdteka.Models
{
    public class InvoiceItemViewModel
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public string DvdName { get; set; }

        public DateTime RentTime { get; set; }

        public DateTime ReturnTime { get; set; }

        public decimal Price { get; set; }
    }
}

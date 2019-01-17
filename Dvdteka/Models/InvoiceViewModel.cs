using System;
using System.Collections.Generic;

namespace Dvdteka.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public DateTime ReturnTime { get; set; }

        public decimal PriceSum { get; set; }

        public List<InvoiceItemViewModel> InvoiceItems { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Data
{
    public class Invoice
    {
        public int Id { get; set; }

        [Display(Name = "Vrijeme povratka")]
        public DateTime ReturnTime { get; set; }

        public int MemberId { get; set; }

        public Member Member { get; set; }

        [Display(Name = "Član")]
        public string MemberName { get; set; }

        public ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}

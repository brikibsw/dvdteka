using System.Collections.Generic;

namespace Dvdteka.Models
{
    public class MemberViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int OpenedRentsSum { get; set; }

        public int ClosedRentsSum { get; set; }

        public decimal PriceSum { get; set; }

        public List<MemberContactViewModel> MemberContacts { get; set; }

        public List<RentViewModel> OpenedRents { get; set; }
    }
}

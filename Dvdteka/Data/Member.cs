using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Data
{
    public class Member
    {
        public Member()
        {
            MemberContacts = new HashSet<MemberContact>();
            Rents = new HashSet<Rent>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime i prezime je obavezan podatak!")]
        [Display(Name = "Ime i prezime")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adresa je obavezan podatak!")]
        [Display(Name = "Adresa")]
        public string Address { get; set; }

        public ICollection<MemberContact> MemberContacts { get; set; }

        public ICollection<Rent> Rents { get; set; }
    }
}

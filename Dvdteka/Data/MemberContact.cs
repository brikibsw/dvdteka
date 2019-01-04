using System.ComponentModel.DataAnnotations;

namespace Dvdteka.Data
{
    public class MemberContact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tip je obavezan podatak!")]
        [Display(Name = "Tip")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Vrijednost je obavezan podatak!")]
        [Display(Name = "Vrijednost")]
        public string Value { get; set; }

        [Required(ErrorMessage = "Član je obavezan podatak!")]
        [Display(Name = "Član")]
        public int MemberId { get; set; }

        public Member Member { get; set; }
    }
}

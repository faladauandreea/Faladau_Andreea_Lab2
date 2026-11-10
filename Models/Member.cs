
using System.ComponentModel.DataAnnotations;

namespace Faladau_Andreea_Lab2.Models
{
    public class Member
    {
        public int ID { get; set; }

        [RegularExpression(
            @"^[A-Z]+[a-zA-Z\s-]*$",
            ErrorMessage = "Prenumele trebuie sa inceapa cu majuscula (ex. Ana, Ana Maria sau Ana-Maria).")]
        [StringLength(30, MinimumLength = 3,
            ErrorMessage = "Prenumele trebuie sa aiba intre 3 si 30 de caractere.")]
        public string? FirstName { get; set; }

        [RegularExpression(
            @"^[A-Z]+[a-z\s]*$",
            ErrorMessage = "Numele trebuie sa inceapa cu majuscula (ex. Popescu sau Ionescu).")]
        [StringLength(30, MinimumLength = 3,
            ErrorMessage = "Numele trebuie sa aiba intre 3 si 30 de caractere.")]
        public string? LastName { get; set; }

        [StringLength(70,
            ErrorMessage = "Adresa nu poate depasi 70 de caractere.")]
        public string? Adress { get; set; }

        public string Email { get; set; } = default!;

        [RegularExpression(
            @"^0[0-9]{3}[-. ]?[0-9]{3}[-. ]?[0-9]{3}$",
            ErrorMessage = "Telefonul trebuie sa inceapa cu 0 si sa aiba formatul 0722-123-123, 0722.123.123 sau 0722 123 123.")]
        public string? Phone { get; set; }

        [Display(Name = "Full Name")]
        public string? FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public ICollection<Borrowing>? Borrowings { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Domain.Entities
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required"), MaxLength(100,ErrorMessage = "Name max length is 100 Characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "The Phone number field is required")]
        [RegularExpression(@"^(?:0|\+98|0098)?9\d{9}$", ErrorMessage = "Invalid Iranian mobile number")]
        public required string Phone { get; set; }

        [StringLength(100, ErrorMessage = "The E-mail length exceeds the limited length")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        public ContactPicture? Picture { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid Date input")]
        public DateTime? Birthday { get; set; }
    }
}

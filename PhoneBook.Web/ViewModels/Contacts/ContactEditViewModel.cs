using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Web.ViewModels.Contacts
{
    public class ContactEditViewModel
    {
        [Required(ErrorMessage = "Contact ID is required")]
        public required int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
        [Display(Name = "Full Name")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "The Phone number field is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(?:0|\+98|0098)?9\d{9}$", ErrorMessage = "Invalid Iranian mobile number")]
        [Display(Name = "Phone Number")]
        public required string Phone { get; set; }

        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [Display(Name = "Birthday")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Current Picture")]
        public string? PictureBase64 { get; set; }

        public string? ReturnUrl { get; set; }
    }
}

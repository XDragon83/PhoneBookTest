using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Services.DTOs.Contact
{
    /// <summary>
    /// Detailed contact DTO used for displaying full contact information with full-size picture
    /// </summary>
    public class ContactDetailsDto
    {
        [Required(ErrorMessage = "Contact ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Contact Name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Contact Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Picture")]
        public string? PictureBase64 { get; set; }

        [Display(Name = "Picture MIME Type")]
        public string? PictureMimeType { get; set; }
    }
}
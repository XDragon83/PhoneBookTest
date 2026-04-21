using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Services.DTOs.Contact
{
    /// <summary>
    /// Minimal contact DTO used for listing/gallery views with essential information and thumbnail
    /// </summary>
    public class ContactMinimalDto
    {
        [Required(ErrorMessage = "Contact ID is required")]
        public required int Id { get; set; }

        [Required(ErrorMessage = "Contact Name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Contact Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public required string Phone { get; set; }

        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Thumbnail")]
        public string? ThumbnailBase64 { get; set; }
    }
}

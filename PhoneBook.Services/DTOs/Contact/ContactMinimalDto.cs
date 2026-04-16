using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Services.DTOs.Contact
{
    public class ContactMinimalDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ThumbnailBase64 {  get; set; }
    }
}

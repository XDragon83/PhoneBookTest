using Microsoft.AspNetCore.Http;

namespace PhoneBook.Services.DTOs.Contact
{
    public class ContactDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }

        public string? PictureBase64 { get; set; }
        public string? PictureMimeType { get; set; }
    }
}
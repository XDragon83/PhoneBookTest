using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Phonebook.Models;

namespace PhoneBook.Models
{
    public class ContactPicture
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key to Contact
        public int ContactId { get; set; }

        [ForeignKey("ContactId")]
        public required Contact Contact { get; set; }

        // The image file stored as a byte array
        public byte[]? ImageData { get; set; }

        // MIME type (image/png, image/jpeg)
        public string? ContentType { get; set; }
    }
}

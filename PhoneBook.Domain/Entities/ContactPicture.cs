using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneBook.Domain.Entities
{
    public class ContactPicture
    {
        private const int MaxImageSizeInBytes = 5 * 1024 * 1024; // 5MB
        [Key]
        public int Id { get; set; }

        // Foreign Key to Contact
        public int ContactId { get; set; }

        [ForeignKey("ContactId")]
        public required Contact Contact { get; set; }

        // The image file stored as a byte array
        [Required(ErrorMessage = "ImageData is required for a ContactPicture")] // Max size 5MB
        [MaxLength(MaxImageSizeInBytes, ErrorMessage = "Image size is larger than the set size")]
        public required byte[] ImageData { get; set; }

        [Required(ErrorMessage = "ThumbnailData isn't generated")]
        public required byte[] ThumbnailData { get; set; }
        // MIME type (image/png, image/jpeg)
        public required string ContentType { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Services.DTOs.Contact
{
    public class ContactEditDto
    {
        public required int Id { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        [Phone]
        public required string Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
    }
}

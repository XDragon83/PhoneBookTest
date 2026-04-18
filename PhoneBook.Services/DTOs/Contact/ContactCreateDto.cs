using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Services.DTOs.Contact
{
    public class ContactCreateDto
    {
        [MaxLength(100)]
        public required string Name {  get; set; }
        [Phone]
        public required string Phone {  get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public IFormFile? Picture {  get; set; }
    }
}

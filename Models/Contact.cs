using System;
using System.ComponentModel.DataAnnotations;

namespace Phonebook.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required, MaxLength(20)]
        public required string Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }
    }
}

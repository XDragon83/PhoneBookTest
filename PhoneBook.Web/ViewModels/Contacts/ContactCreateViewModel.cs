namespace PhoneBook.Web.ViewModels.Contacts
{
    public class ContactCreateViewModel
    {
        public required string Name { get; set; }
        public required string Phone { get; set; }

        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public IFormFile? Picture { get; set; }
        public string? ReturnUrl { get; set; } = string.Empty;
    }
}

namespace PhoneBook.Web.ViewModels.Contacts
{
    public class ContactEditViewModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string? PictureBase64 { get; set; }
        public string? ReturnUrl { get; set; }
    }
}

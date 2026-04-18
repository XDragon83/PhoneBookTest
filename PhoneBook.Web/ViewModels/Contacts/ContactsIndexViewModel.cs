using PhoneBook.Services.DTOs.Contact;

namespace PhoneBook.Web.ViewModels.Contacts
{
    public class ContactsIndexViewModel
    {
        required public IEnumerable<ContactMinimalDto> Contacts {  get; set; }
        public int ContactsCount => Contacts.Count();
    }
}

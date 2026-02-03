using PhoneBook.Models;

namespace PhoneBook.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Contact GetContact(int id);
        IEnumerable<Contact> GetAllContacts();
        void AddContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(int id);
        ContactPicture GetContactPicture(int id);
        ContactPicture GetContactPicture(Contact contact);
        IEnumerable<ContactPicture> GetAllContactPictures();
        void AddContactPicture(Contact contact, ContactPicture contactPicture);
        void UpdateContactPicture(Contact contact, ContactPicture contactPicture);
        void UpdateContactPicture(int id, ContactPicture contactPicture);
        void DeleteContactPicture(int id);
        void DeleteContactPicture(Contact contact);
    }
}

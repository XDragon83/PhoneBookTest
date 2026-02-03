using PhoneBook.Models;
using PhoneBook.Repositories.Interfaces;

namespace PhoneBook.Repositories
{
    public class ContactRepository : IContactRepository
    {
        public void AddContact(Contact contact)
        {
            throw new NotImplementedException();
        }

        public void AddContactPicture(Contact contact, ContactPicture contactPicture)
        {
            throw new NotImplementedException();
        }

        public void DeleteContact(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteContactPicture(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteContactPicture(Contact contact)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContactPicture> GetAllContactPictures()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            throw new NotImplementedException();
        }

        public Contact GetContact(int id)
        {
            throw new NotImplementedException();
        }

        public ContactPicture GetContactPicture(int id)
        {
            throw new NotImplementedException();
        }

        public ContactPicture GetContactPicture(Contact contact)
        {
            throw new NotImplementedException();
        }

        public void UpdateContact(Contact contact)
        {
            throw new NotImplementedException();
        }

        public void UpdateContactPicture(Contact contact, ContactPicture contactPicture)
        {
            throw new NotImplementedException();
        }

        public void UpdateContactPicture(int id, ContactPicture contactPicture)
        {
            throw new NotImplementedException();
        }
    }
}

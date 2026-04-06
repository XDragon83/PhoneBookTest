using PhoneBook.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace PhoneBook.Services.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact?> GetByIdAsync(int id);

        Task CreateAsync(Contact contact, IFormFile? pictureFile);
        Task UpdateAsync(Contact contact, IFormFile? pictureFile);
        Task DeleteAsync(int id);

        Task UpdatePictureAsync(int contactId, IFormFile pictureFile);
        Task RemovePictureAsync(int contactId);
    }
}
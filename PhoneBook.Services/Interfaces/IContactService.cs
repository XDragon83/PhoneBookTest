using PhoneBook.Domain.Models;
using PhoneBook.Services.DTOs.Contact;
using Microsoft.AspNetCore.Http;

namespace PhoneBook.Services.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ContactMinimalDto>> GetAllAsync();
        Task<ContactDetailsDto?> GetByIdAsync(int id);
        Task CreateAsync(ContactCreateDto contact, IFormFile? pictureFile);
        Task UpdateAsync(ContactEditDto contact);
        Task DeleteAsync(int id);

        Task UpdatePictureAsync(int contactId, IFormFile pictureFile);
        Task RemovePictureAsync(int contactId);
    }
}
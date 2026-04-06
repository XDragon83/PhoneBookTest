using PhoneBook.Domain.Models;

namespace PhoneBook.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact?> GetByIdAsync(int id);
        Task<IEnumerable<Contact>> GetAllAsync();
        Task AddAsync(Contact contact);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(Contact contact);
    }
}
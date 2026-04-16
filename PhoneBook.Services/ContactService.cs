using PhoneBook.Domain.Models;
using PhoneBook.Services.DTOs.Contact;
using PhoneBook.Repositories.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using PhoneBook.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace PhoneBook.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repo;

        public ContactService(IContactRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ContactMinimalDto>> GetAllAsync()
        {
            IEnumerable<Contact> contacts = await _repo.GetAllAsync();
            List<ContactMinimalDto> contactDtos = new List<ContactMinimalDto>();
            ContactMinimalDto? temp = null;
            foreach (Contact contact in contacts)
            {
                temp = new ContactMinimalDto()
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Phone = contact.Phone,
                    Birthday = contact.Birthday,
                    Email = contact.Email,
                    ThumbnailBase64 = contact.Picture != null ?
                    Convert.ToBase64String(contact.Picture.ThumbnailData) : null
                };
                contactDtos.Add(temp);
            }

            return contactDtos;
        }

        public async Task<ContactDetailsDto?> GetByIdAsync(int id)
        {
            Contact? contact = await _repo.GetByIdAsync(id);
            if (contact != null)
            {
                ContactDetailsDto contactDto = new ContactDetailsDto()
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Phone = contact.Phone,
                    Birthday = contact.Birthday,
                    Email = contact.Email,
                    PictureBase64 = contact.Picture != null ?
                        Convert.ToBase64String(contact.Picture.ImageData) : null,
                    PictureMimeType = contact.Picture != null && contact.Picture.ContentType != null ?
                        contact.Picture.ContentType : null

                };
                return contactDto;
            }
            return null;
        }

        public async Task CreateAsync(ContactCreateDto contact, IFormFile? pictureFile)
        {
            Contact newContact = new() { Name = contact.Name, Phone = contact.Phone, Email = contact.Email, Birthday = contact.Birthday, };

            if (pictureFile != null)
            {
                var bytes = await ConvertFileToBytesAsync(pictureFile);
                var thumbnail = GenerateThumbnail(bytes);
                ContactPicture contactPicture = new()
                {
                    Contact = newContact,
                    ImageData = bytes,
                    ThumbnailData = thumbnail,
                    ContentType = pictureFile.ContentType
                };
                newContact.Picture = contactPicture;
            }


            await _repo.AddAsync(newContact);
        }

        public async Task UpdateAsync(ContactEditDto contact)
        {
            var existing = await _repo.GetByIdAsync(contact.Id);
            if (existing == null)
                throw new Exception("Contact not found");

            existing.Name = contact.Name;
            existing.Phone = contact.Phone;
            existing.Email = contact.Email;
            existing.Birthday = contact.Birthday;

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var contact = await _repo.GetByIdAsync(id);
            if (contact == null)
                return;

            await _repo.DeleteAsync(contact);
        }

        public async Task UpdatePictureAsync(int contactId, IFormFile pictureFile)
        {
            var contact = await _repo.GetByIdAsync(contactId);
            if (contact == null)
                throw new Exception("Contact not found");

            var bytes = await ConvertFileToBytesAsync(pictureFile);
            var thumbnail = GenerateThumbnail(bytes);

            contact.Picture = new ContactPicture
            {
                Contact = contact,
                ContentType = pictureFile.ContentType,
                ImageData = bytes,
                ThumbnailData = thumbnail
            };

            await _repo.UpdateAsync(contact);
        }

        public async Task RemovePictureAsync(int contactId)
        {
            var contact = await _repo.GetByIdAsync(contactId);
            if (contact == null)
                throw new Exception("Contact not found");

            contact.Picture = null;

            await _repo.UpdateAsync(contact);
        }

        // -----------------------------
        // Helpers (business logic)
        // -----------------------------

        private async Task<byte[]> ConvertFileToBytesAsync(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }

        private byte[] GenerateThumbnail(byte[] original)
        {
            using var image = Image.Load(original);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(200, 0) // 200px width
            }));

            using var ms = new MemoryStream();
            image.SaveAsJpeg(ms);
            return ms.ToArray();
        }
    }
}
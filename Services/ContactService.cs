using PhoneBook.Models;
using PhoneBook.Repositories.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using PhoneBook.Services.Interfaces;

namespace PhoneBook.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repo;

        public ContactService(IContactRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Contact?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task CreateAsync(Contact contact, IFormFile? pictureFile)
        {
            if (pictureFile != null)
            {
                var bytes = await ConvertFileToBytesAsync(pictureFile);
                var thumbnail = GenerateThumbnail(bytes);

                contact.Picture = new ContactPicture
                {
                    Contact = contact,
                    ImageData = bytes,
                    ThumbnailData = thumbnail,
                    ContentType = pictureFile.ContentType
                };
            }

            await _repo.AddAsync(contact);
        }

        public async Task UpdateAsync(Contact contact, IFormFile? pictureFile)
        {
            var existing = await _repo.GetByIdAsync(contact.Id);
            if (existing == null)
                throw new Exception("Contact not found");

            existing.Name = contact.Name;
            existing.Phone = contact.Phone;

            if (pictureFile != null)
            {
                var bytes = await ConvertFileToBytesAsync(pictureFile);
                var thumbnail = GenerateThumbnail(bytes);

                existing.Picture = new ContactPicture
                {
                    Contact = contact,
                    ContentType = pictureFile.ContentType,
                    ImageData = bytes,
                    ThumbnailData = thumbnail
                };
            }

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
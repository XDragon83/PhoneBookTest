using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;
using System.Net.Cache;
using System.Threading.Tasks;

namespace PhoneBook.Controllers
{
    public class ContactsController : Controller
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        private async Task<byte[]> ConvertFileToBytes(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }


        // LIST
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contacts.ToListAsync());
        }

        public IActionResult GetImage(int Id)
        {
            ContactPicture? picture = _context.ContactPictures.FirstOrDefault(p => p.ContactId == Id);
            if (picture == null || picture.ImageData == null)
                return File("~/images/default_contact.png", "image/png");

            return File(picture.ImageData, picture.ContentType);
        }

        public IActionResult DeleteImage(int contactId)
        {
            ContactPicture? picture = _context.ContactPictures.FirstOrDefault(p => p.ContactId == contactId);

            if (picture != null)
            {
                _context.ContactPictures.Remove(picture);
                _context.SaveChanges();
                return RedirectToAction(nameof(Edit), new { id = contactId });
            }
            else
            {
                return RedirectToAction(nameof(Edit), new { id = contactId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(int contactId, IFormFile file)
        {
            if (!file.ContentType.StartsWith("image/"))
            {
                return BadRequest("Only image files are allowed");
            }

            if (file == null || file.Length == 0)
                return RedirectToAction(nameof(Edit), new { id = contactId });

            ContactPicture? picture = _context.ContactPictures.FirstOrDefault(p => p.ContactId == contactId);
            byte[] fileBytes = await ConvertFileToBytes(file);

            if (picture != null)
            {
                picture.ImageData = fileBytes;
                picture.ContentType = file.ContentType;
                _context.ContactPictures.Update(picture);
            }
            else
            {
                Contact? contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId);
                if (contact == null) return NotFound();
                picture = new ContactPicture
                {
                    ContactId = contactId,
                    Contact = contact,
                    ImageData = fileBytes,
                    ContentType = file.ContentType,
                    ThumbnailData = fileBytes
                };
                _context.ContactPictures.Add(picture);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { id = contactId });
        }


        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null) return NotFound();

            return View(contact);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Contact contact, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }
            if (file != null && file.ContentType.StartsWith("image/"))
            {
                contact.Picture = new ContactPicture
                {
                    Contact = contact,
                    ContentType = file.ContentType,
                    ImageData = await ConvertFileToBytes(file),
                    ThumbnailData = await ConvertFileToBytes(file)
                };
            }

            _context.Add(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contact = await _context.Contacts.Include(c => c.Picture).FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null) return NotFound();

            return View(contact);
        }

        // EDIT (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Contact contact)
        {
            if (id != contact.Id) return NotFound();
            if (!ModelState.IsValid) return View(contact);

            _context.Update(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(int id, Contact contact, IFormFile? file, bool deletePicture)
        //{
        //    if (id != contact.Id) return NotFound();

        //    if (ModelState.IsValid)
        //    {
        //        var existingContact = await _context.Contacts
        //            .Include(c => c.Picture)
        //            .FirstOrDefaultAsync(c => c.Id == id);

        //        if (existingContact == null)
        //            return NotFound();

        //        existingContact.Name = contact.Name;
        //        existingContact.Phone = contact.Phone;
        //        existingContact.Email = contact.Email;
        //        existingContact.Birthday = contact.Birthday;

        //        // DELETE picture
        //        if (deletePicture && existingContact.Picture != null)
        //        {
        //            _context.ContactPictures.Remove(existingContact.Picture);
        //            existingContact.Picture = null;
        //        }

        //        // UPLOAD new picture
        //        if (file != null)
        //        {
        //            if (existingContact.Picture != null)
        //            {
        //                existingContact.Picture.ContentType = file.ContentType;
        //                existingContact.Picture.ImageData = await ConvertFileToBytes(file);
        //            }
        //            else
        //            {
        //                existingContact.Picture = new ContactPicture
        //                {
        //                    Contact = existingContact,
        //                    ContentType = file.ContentType,
        //                    ImageData = await ConvertFileToBytes(file),
        //                    ContactId = existingContact.Id
        //                };
        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(contact);
        //}

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var contact = await _context.Contacts.Include(c => c.Picture).FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null) return NotFound();

            return View(contact);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return NotFound();
            _context.Contacts.Remove(contact);

            var contactPicture = await _context.ContactPictures.FirstOrDefaultAsync(p => p.ContactId == id);
            if (contactPicture != null)
            {
                _context.ContactPictures.Remove(contactPicture);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

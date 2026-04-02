using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;
using PhoneBook.Services.Interfaces;

namespace PhoneBook.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactService _service;

        public ContactsController(IContactService service)
        {
            _service = service;
        }


        // LIST
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }
        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var contact = await _service.GetByIdAsync(id.Value);

            if (contact == null)
                return NotFound();

            return View(contact);
        }

        // CREATE (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Contact contact, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            await _service.CreateAsync(contact, file);

            return RedirectToAction(nameof(Index));
        }

        // DELETE (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return NotFound();

            Contact? contact = await _service.GetByIdAsync(id.Value);

            if (contact == null) return NotFound();

            return View(contact);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id.HasValue)
            {
                await _service.DeleteAsync(id.Value);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        // EDIT (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                Contact? contact = await _service.GetByIdAsync(id.Value);
                if (contact != null)
                {
                    return View(contact);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id.HasValue)
            {
                await _service.RemovePictureAsync(id.Value);
                return RedirectToAction(nameof(Edit), new { id = id });
            }
            return NotFound();
        }

        // EDIT (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Contact contact)
        {
            if (!ModelState.IsValid) return View(contact);
            if (await _service.GetByIdAsync(id) == null || id != contact.Id) return NotFound();
            await _service.UpdateAsync(contact, null);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(int? id, IFormFile? file)
        {
            if (id.HasValue && file != null)
            {
                try
                {
                    await _service.UpdatePictureAsync(id.Value, file);
                    return RedirectToAction(nameof(Edit), new { id = id });
                }
                catch {
                    return NotFound();
                }
            }
            return NotFound();

        }
    }
}

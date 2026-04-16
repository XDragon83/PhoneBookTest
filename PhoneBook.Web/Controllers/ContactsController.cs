using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Domain.Models;
using PhoneBook.Services.DTOs.Contact;
using PhoneBook.Services.Interfaces;
using System.Net.Http.Headers;

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

        public async Task<IActionResult> Gallery()
        {
            return View(await _service.GetAllAsync());
        }

        // Set Language
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = System.DateTimeOffset.UtcNow.AddMinutes(2) }
            );

            return LocalRedirect(returnUrl);
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id, string? returnUrl)
        {
            if (!id.HasValue)
                return NotFound();

            var contact = await _service.GetByIdAsync(id.Value);
            ViewBag.ReturnUrl = returnUrl != null ? returnUrl : "/";

            if (contact == null)
                return NotFound();

            return View(contact);
        }

        // CREATE (GET)
        [HttpGet]
        public IActionResult Create(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl != null ? returnUrl : "/";
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> Create(ContactCreateDto contact, IFormFile? file, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            await _service.CreateAsync(contact, file);

            return LocalRedirect(returnUrl != null ? returnUrl : "/");
        }

        // DELETE (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string? returnUrl)
        {
            if (!id.HasValue) return NotFound();

            var contact = await _service.GetByIdAsync(id.Value);
            ViewBag.ReturnUrl = returnUrl != null ? returnUrl : "/";

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
        public async Task<IActionResult> Edit(int? id, string? returnUrl)
        {
            if (id.HasValue)
            {
                var contact = await _service.GetByIdAsync(id.Value);
                ViewBag.ReturnUrl = returnUrl != null ? returnUrl : "/";
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
        public async Task<IActionResult> Edit(int id, ContactEditDto contact)
        {
            if (!ModelState.IsValid) return View(contact);
            if (await _service.GetByIdAsync(id) == null || id != contact.Id) return NotFound();
            await _service.UpdateAsync(contact);

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
                catch
                {
                    return NotFound();
                }
            }
            return NotFound();

        }
    }
}
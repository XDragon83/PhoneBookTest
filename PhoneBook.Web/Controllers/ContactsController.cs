using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Services.DTOs.Contact;
using PhoneBook.Services.Interfaces;
using PhoneBook.Web.ViewModels.Contacts;

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
            var contacts = await _service.GetAllAsync();
            var viewModel = new ContactsIndexViewModel
            {
                Contacts = contacts,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Gallery()
        {
            var contacts = await _service.GetAllAsync();
            var viewModel = new ContactGalleryViewModel
            {
                Contacts = contacts,
            };
            return View(viewModel);
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

            var vm = new ContactDetailsViewModel
            {
                Contact = contact
            };

            return View(vm);
        }

        // CREATE (GET)
        [HttpGet]
        public IActionResult Create(ContactCreateViewModel contact)
        {
            ViewBag.ReturnUrl = contact.ReturnUrl != null && contact.ReturnUrl != string.Empty ? contact.ReturnUrl : "/";
            return View();
        }

        // CREATE (POST)
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateConfirmed(ContactCreateViewModel contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }
            ContactCreateDto contactCreateDto = new()
            {
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Birthday = contact.Birthday,
                Picture = contact.Picture,
            };
            await _service.CreateAsync(contactCreateDto);
            string returnUrl = contact.ReturnUrl != null && contact.ReturnUrl != string.Empty ?
                contact.ReturnUrl : "/";
            return LocalRedirect(returnUrl);
        }

        // DELETE (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string? returnUrl)
        {
            if (!id.HasValue) 
                return NotFound();

            var contact = await _service.GetByIdAsync(id.Value);

            if (contact == null) 
                return NotFound();

            ViewBag.ReturnUrl = returnUrl ?? "/";

            var viewModel = new ContactDeleteViewModel
            {
                Contact = contact
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id, string? returnUrl)
        {
            if (!id.HasValue)
                return NotFound();

            try
            {
                var contact = await _service.GetByIdAsync(id.Value);
                if (contact == null)
                    return NotFound();

                await _service.DeleteAsync(id.Value);

                // Redirect to return URL if provided, otherwise to Index
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return LocalRedirect(returnUrl);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while deleting the contact.");
            }
        }

        // EDIT (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string? returnUrl)
        {
            if (id.HasValue)
            {
                var contact = await _service.GetByIdAsync(id.Value);
                if (contact != null)
                {
                    ViewBag.ReturnUrl = returnUrl != null && returnUrl != string.Empty ? returnUrl : "/";
                    var vm = new ContactEditViewModel
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Phone = contact.Phone,
                        Email = contact.Email,
                        Birthday = contact.Birthday,
                        PictureBase64 = contact.PictureBase64,
                        ReturnUrl = returnUrl != null && returnUrl != string.Empty ? returnUrl : "/"
                    };
                    return View(vm);
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
        public async Task<IActionResult> Edit(ContactEditViewModel contact)
        {
            if (!ModelState.IsValid) return View(contact);
            if (await _service.GetByIdAsync(contact.Id) == null) return NotFound();
            ContactEditDto contactUpdateDto = new()
            {
                Id = contact.Id,
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Birthday = contact.Birthday
            };
            await _service.UpdateAsync(contactUpdateDto);
            return LocalRedirect(contact.ReturnUrl != null && contact.ReturnUrl != string.Empty ? contact.ReturnUrl : "/");
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(int? id, IFormFile? file)
        {
            if (id.HasValue && file != null)
            {
                try
                {
                    await _service.UpdatePictureAsync(id.Value, file);
                    return RedirectToAction(nameof(Edit), new { id });
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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rule.BL.Models;
using Rule.BL.Services;
using Rule.Common.Extensions;
using Rule.DAL.Entities;
using System.Security.Claims;

namespace Rule.UI.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersService _service;

        public UsersController(UsersService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await _service.GetUserByUsername(model.Username);

                if (user == null)
                {
                    ModelState.AddModelError("", "Користувача з таким username не знайдено.");
                    return View(model);
                }

                if (user.Username != model.Username || user.Password != model.Password)
                {
                    ModelState.AddModelError("", "Неправильний логін або пароль.");
                    return View(model);
                }

                // Create the identity from the user info
                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Виникла помилка під час входу.");
                return View(model);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "Logout successful";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> RegistrationAsync(UsersDTO newUsers)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(newUsers);
            }

            try
            {
                var createdUsers = await _service.CreateAsync(newUsers);
                TempData["SuccessMessage"] = "Користувач успішно зареєструвався!";
                return RedirectToAction("Index");
            }
            catch (DuplicateItemException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (ServerErrorException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return View(newUsers);
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<UsersDTO>>> GetAllAsync()
        {
            try
            {
                var getAllUsers = await _service.GetAllAsync();
                return View(getAllUsers);
            }
            catch (ServerErrorException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult<UsersDTO>> GetByIdAsync(int id)
        {
            try
            {
                var getByUsers = await _service.GetByIdAsync(id);
                return View(getByUsers);
            }
            catch (InvalidIdException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (ServerErrorException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        #region Http Get & Post method Update
        [HttpGet]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            try
            {
                var updateAsUsers = await _service.GetByIdAsync(id);
                if (updateAsUsers == null || updateAsUsers.Id != id)
                {
                    TempData["ErrorMessage"] = "Фонд з таким ідентифікатором не знайдено";
                    return RedirectToAction("Index");
                }
                return View(updateAsUsers);
            }
            catch (InvalidIdException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (ServerErrorException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult<UsersDTO>> UpdateAsync(UsersDTO updateUs)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return RedirectToAction("Index");
            }

            try
            {
                await _service.UpdateAsync(updateUs);
                TempData["SuccessMessage"] = "Фонд успішно оновлено!";
            }
            catch (InvalidIdException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (DuplicateItemException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (ServerErrorException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Http Get & Post method Delete
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var deleteUs = await _service.GetByIdAsync(id);
                return View(deleteUs);
            }
            catch (InvalidIdException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (ServerErrorException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["SuccessMessage"] = "Фонд успішно видалено!";
            }
            catch (InvalidIdException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (ServerErrorException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}

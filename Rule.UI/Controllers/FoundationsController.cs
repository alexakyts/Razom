using Microsoft.AspNetCore.Mvc;
using Rule.BL.Models;
using Rule.BL.Services;
using Rule.Common.Extensions;

namespace Rule.UI.Controllers
{
    public class FoundationsController : Controller
    {
        private readonly FoundationsService _service;

        public FoundationsController(FoundationsService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(FoundationsDTO newFoundations, IFormFile pictureFile)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(newFoundations);
            }

            try
            {
                var createdFoundations = await _service.CreateAsync(newFoundations);
                TempData["SuccessMessage"] = "Фонд успішно створено!";
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

            return View(newFoundations);
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<FoundationsDTO>>> GetAllAsync()
        {
            try
            {
                var foundations = await _service.GetAllAsync();
                return View(foundations);
            }
            catch (ServerErrorException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult<FoundationsDTO>> GetByIdAsync(int id)
        {
            try
            {
                var foundations = await _service.GetByIdAsync(id);
                return View(foundations);
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
                var foundations = await _service.GetByIdAsync(id);
                if (foundations == null || foundations.Id != id)
                {
                    TempData["ErrorMessage"] = "Фонд з таким ідентифікатором не знайдено";
                    return RedirectToAction("Index");
                }
                return View(foundations);
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
        public async Task<ActionResult<FoundationsDTO>> UpdateAsync(FoundationsDTO update)
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
                await _service.UpdateAsync(update);
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
                var foundations = await _service.GetByIdAsync(id);
                return View(foundations);
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

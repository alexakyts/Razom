using Microsoft.AspNetCore.Mvc;
using Rule.BL.Models;
using Rule.BL.Services;
using Rule.Common.Extensions;

namespace Rule.UI.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostsService _service;

        public PostsController(PostsService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(PostsDTO newPosts)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(newPosts);
            }

            try
            {
                var createdPosts = await _service.CreateAsync(newPosts);
                TempData["SuccessMessage"] = "Пост успішно створено!";
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

            return View(newPosts);
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<PostsDTO>>> GetAllAsync()
        {
            try
            {
                var getAllPosts = await _service.GetAllAsync();
                return View(getAllPosts);
            }
            catch (ServerErrorException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult<PostsDTO>> GetByIdAsync(int id)
        {
            try
            {
                var getByPosts = await _service.GetByIdAsync(id);
                return View(getByPosts);
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
                var updateAsPosts = await _service.GetByIdAsync(id);
                if (updateAsPosts == null || updateAsPosts.Id != id)
                {
                    TempData["ErrorMessage"] = "Пост з таким ідентифікатором не знайдено";
                    return RedirectToAction("Index");
                }
                return View(updateAsPosts);
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
        public async Task<ActionResult<PostsDTO>> UpdateAsync(PostsDTO updatePos)
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
                await _service.UpdateAsync(updatePos);
                TempData["SuccessMessage"] = "Пост успішно оновлено!";
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
                var deletePos = await _service.GetByIdAsync(id);
                return View(deletePos);
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
                TempData["SuccessMessage"] = "Пост успішно видалено!";
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


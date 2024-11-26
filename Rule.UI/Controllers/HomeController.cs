using Microsoft.AspNetCore.Mvc;

namespace Rule.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Mental()
        {
            return View();
        }

        public IActionResult Zbory()
        {
            return View();
        }

        public IActionResult House()
        {
            return View();
        }
        
        public IActionResult Pritulki()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendPhoneNumber(string phoneNumber)
        {
            TempData["Message"] = "Ваш номер успішно передано, з вами скоро зв'яжуться";
            return RedirectToAction("Index");
        }
    }
}

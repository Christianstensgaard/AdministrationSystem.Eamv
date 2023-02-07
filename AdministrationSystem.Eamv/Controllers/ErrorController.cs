using Microsoft.AspNetCore.Mvc;

namespace AdministrationSystem.Eamv.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View("404");
        }
    }
}

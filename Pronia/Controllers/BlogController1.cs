using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class BlogController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

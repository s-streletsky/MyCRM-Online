using Microsoft.AspNetCore.Mvc;

namespace MyCRM_Online.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {            
            return View();
        }
    }
}

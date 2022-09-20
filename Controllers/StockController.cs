using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

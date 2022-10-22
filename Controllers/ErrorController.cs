using Microsoft.AspNetCore.Mvc;
using MyCRM_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

using Microsoft.AspNetCore.Mvc;
using MyCRM_Online.Db;
using MyCRM_Online.Models;
using System.Diagnostics;

namespace MyCRM_Online.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ClientsRepo clientsRepo = new ClientsRepo();
        CountriesRepo countriesRepo = new CountriesRepo();
        ShippingMethodsRepo shippingMethodsRepo = new ShippingMethodsRepo();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Clients()
        {
            var clients = clientsRepo.GetAll();
            ViewBag.Clients = clients;

            return View();
        }

        public IActionResult EditClient()
        {
            var countries = countriesRepo.GetAll();
            var shippingMethods = shippingMethodsRepo.GetAll();
            ViewBag.Countries = countries;
            ViewBag.ShippingMethods = shippingMethods;

            return View(); 
        }

        [HttpPost]
        public IActionResult AddClient([FromForm]Client client)
        {
            client.Date = DateTime.Now;
            clientsRepo.Add(client);

            return RedirectToAction("Clients");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
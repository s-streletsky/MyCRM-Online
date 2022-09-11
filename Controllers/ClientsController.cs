using Microsoft.AspNetCore.Mvc;
using MyCRM_Online.Db;
using MyCRM_Online.Models;
using System.Diagnostics;

namespace MyCRM_Online.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ILogger<ClientsController> _logger;
        ClientsRepo clientsRepo = new ClientsRepo();
        CountriesRepo countriesRepo = new CountriesRepo();
        ShippingMethodsRepo shippingMethodsRepo = new ShippingMethodsRepo();

        public ClientsController(ILogger<ClientsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var clients = clientsRepo.GetAll();
            ViewBag.Clients = clients;

            return View();
        }

        //public IActionResult Clients()
        //{
        //    var clients = clientsRepo.GetAll();
        //    ViewBag.Clients = clients;

        //    return View();
        //}

        public IActionResult Edit()
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
    }
}
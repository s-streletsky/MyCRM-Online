using Microsoft.AspNetCore.Mvc;
using MyCRM_Online.Db;
using MyCRM_Online.Models;
using System.Diagnostics;
using System.Drawing.Printing;

namespace MyCRM_Online.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ILogger<ClientsController> _logger;
        readonly DatabaseContext databaseContext;

        public ClientsController(ILogger<ClientsController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            this.databaseContext = databaseContext;
        }

        public IActionResult Index() 
        {
            var clients = databaseContext.Clients.ToList();
            ViewBag.Clients = clients;

            return View();
        }

        //public IActionResult Add()
        //{
        //    return View();
        //}

        public IActionResult Create()
        {
            GetCountriesAndShippingMethodsLists();

            return View(); 
        }

        [HttpPost]
        public IActionResult Create([FromForm]Client client)
        {
            client.Date = DateTime.Now;
            databaseContext.Clients.Add(client);
            databaseContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            GetCountriesAndShippingMethodsLists();

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var client = databaseContext.Clients.Find(id);

            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Client client)
        {
            var temp = client;
            if (ModelState.IsValid)
            {
                databaseContext.Clients.Update(client);
                databaseContext.SaveChanges();
                //TempData["ResultOk"] = "Data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [Route("")]
        [HttpPost]
        public IActionResult Delete([FromForm] int? id)
        {
            databaseContext.Clients.Remove(new Client() { Id = id });
            databaseContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private void GetCountriesAndShippingMethodsLists()
        {
            var countries = databaseContext.Countries.ToList();
            var shippingMethods = databaseContext.ShippingMethods.ToList();
            ViewBag.Countries = countries;
            ViewBag.ShippingMethods = shippingMethods;
        }
    }
}
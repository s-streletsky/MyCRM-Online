using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyCRM_Online.Db;
using MyCRM_Online.Models;
using MyCRM_Online.ViewModels;
using System.Diagnostics;
using System.Drawing.Printing;

namespace MyCRM_Online.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ILogger<ClientsController> logger;
        private readonly DataContext databaseContext;
        private readonly IMapper mapper;

        public ClientsController(ILogger<ClientsController> logger, DataContext databaseContext, IMapper mapper)
        {
            this.logger = logger;
            this.databaseContext = databaseContext;
            this.mapper = mapper;
        }

        public IActionResult Index() 
        {
            var source = databaseContext.Clients.ToList();
            var clients = mapper.Map<List<ClientsViewModel>>(source);

            ViewBag.Clients = clients;

            return View();
        }

        public IActionResult Create()
        {
            GetCountriesList();
            GetShippingMethodsList();

            return View(); 
        }

        [HttpPost]
        public IActionResult Create([FromForm]ClientCreateViewModel client)
        {
            client.Date = DateTime.UtcNow;
            var newClient = mapper.Map<Client>(client);
            databaseContext.Clients.Add(newClient);
            databaseContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            GetCountriesList();
            GetShippingMethodsList();

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var source = databaseContext.Clients.Find(id);
            var client = mapper.Map<ClientEditViewModel>(source);

            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ClientEditViewModel client)
        {
            if (ModelState.IsValid)
            {
                var entity = databaseContext.Clients.Single<Client>(c => c.Id == client.Id);
                mapper.Map(client, entity);
                databaseContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [Route("")]
        [HttpPost]
        public IActionResult Delete([FromForm]int? id)
        {
            databaseContext.Clients.Remove(new Client() { Id = id });
            databaseContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private void GetCountriesList()
        {
            var countries = databaseContext.Countries.ToList();            
            ViewBag.Countries = countries;            
        }

        private void GetShippingMethodsList()
        {
            var shippingMethods = databaseContext.ShippingMethods.ToList();
            ViewBag.ShippingMethods = shippingMethods;
        }
    }
}
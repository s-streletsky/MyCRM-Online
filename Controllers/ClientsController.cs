using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyCRM_Online.Db;
using MyCRM_Online.Models;
using MyCRM_Online.ViewModels;
using System.Diagnostics;
using System.Drawing.Printing;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ILogger<ClientsController> logger;
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public ClientsController(ILogger<ClientsController> logger, DataContext dataContext, IMapper mapper)
        {
            this.logger = logger;
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public IActionResult Index() 
        {
            var source = dataContext.Clients.ToList();
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
            dataContext.Clients.Add(newClient);
            dataContext.SaveChanges();

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
            var source = dataContext.Clients.Find(id);
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
                var entity = dataContext.Clients.Single<Client>(c => c.Id == client.Id);
                mapper.Map(client, entity);
                dataContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [Route("")]
        [HttpPost]
        public IActionResult Delete([FromForm]int? id)
        {
            dataContext.Clients.Remove(new Client() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private void GetCountriesList()
        {
            var countries = dataContext.Countries.ToList();            
            ViewBag.Countries = countries;            
        }

        private void GetShippingMethodsList()
        {
            var shippingMethods = dataContext.ShippingMethods.ToList();
            ViewBag.ShippingMethods = shippingMethods;
        }
    }
}
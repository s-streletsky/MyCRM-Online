using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MyCRM_Online.Db;
using MyCRM_Online.Models;
using MyCRM_Online.Models.Entities;
using MyCRM_Online.ViewModels.Clients;
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

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            IQueryable<ClientEntity> source = dataContext.Clients;
            var totalCount = await source.CountAsync();
            var entities = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var clients = mapper.Map<List<ClientViewModel>>(entities);

            var pageInfo = new PageInfo<ClientViewModel>(totalCount, page, pageSize, clients);

            return View(pageInfo);
        }

        public IActionResult Create()
        {
            SetAllCountriesListToViewBag();
            SetAllShippingMethodsListToViewBag();

            return View(); 
        }

        [HttpPost]
        public IActionResult Create([FromForm]ClientCreateViewModel client)
        {
            var newClient = mapper.Map<ClientEntity>(client);
            newClient.Date = DateTime.UtcNow;
            dataContext.Clients.Add(newClient);
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit([FromQuery] int? clientId)
        {
            SetAllCountriesListToViewBag();
            SetAllShippingMethodsListToViewBag();

            if (clientId == null || clientId == 0)
            {
                return NotFound();
            }
            var source = dataContext.Clients.Find(clientId);
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
                var entity = dataContext.Clients.Single<ClientEntity>(c => c.Id == client.Id);
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
            dataContext.Clients.Remove(new ClientEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Profile(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var source = dataContext.Clients.Find(id);
            var client = mapper.Map<ClientProfileViewModel>(source);

            if (client == null)
            {
                return NotFound();
            }

            client.OrdersQuantity = dataContext.Orders.Where(o => o.ClientId == id).Count();
            client.PaymentsTotal = dataContext.Payments.Where(p => p.ClientId == id).Sum(p => p.Amount);

            return View(client);
        }

        private void SetAllCountriesListToViewBag()
        {
            var countries = dataContext.Countries.ToList();            
            ViewBag.Countries = countries;            
        }

        private void SetAllShippingMethodsListToViewBag()
        {
            var shippingMethods = dataContext.ShippingMethods.ToList();
            ViewBag.ShippingMethods = shippingMethods;
        }
    }
}
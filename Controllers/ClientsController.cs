using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MyCRM_Online.Db;
using MyCRM_Online.Extensions;
using MyCRM_Online.Models;
using MyCRM_Online.Models.Entities;
using MyCRM_Online.ViewModels.Clients;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ILogger<ClientsController> logger;
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly HttpClient httpClient;

        public ClientsController(ILogger<ClientsController> logger, DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider, IHttpClientFactory factory)
        {
            this.logger = logger;
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
            this.httpClient = factory.CreateClient("apiClient");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageInfo = new PageInfo<ClientViewModel>();

            using (var response = await httpClient.GetAsync($"/api/clients?page={page}"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                pageInfo = JsonConvert.DeserializeObject<PageInfo<ClientViewModel>>(apiResponse);
            }

            return View(pageInfo);
        }

        public IActionResult Create()
        {
            SetAllCountriesListToViewBag();
            SetAllShippingMethodsListToViewBag();

            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ClientCreateViewModel client)
        {
            if (!ModelState.IsValid)
            {
                SetAllCountriesListToViewBag();
                SetAllShippingMethodsListToViewBag();

                return View(client);                
            }

            var serializedClient = JsonConvert.SerializeObject(client, Formatting.Indented);
            var httpContent = new StringContent(serializedClient, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync($"/api/clients", httpContent))
            {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int? id)
        {
            if (id == null || id < 1) {
                return NotFound();
            }

            ClientEditViewModel client;

            using (var response = await httpClient.GetAsync($"/api/clients/{id}"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<ClientEditViewModel>(apiResponse);
            }

            SetAllCountriesListToViewBag();
            SetAllShippingMethodsListToViewBag();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, ClientEditViewModel client)
        {
            if (client.Id != id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                SetAllCountriesListToViewBag();
                SetAllShippingMethodsListToViewBag();

                return View(client);                
            }

            var serializedClient = JsonConvert.SerializeObject(client, Formatting.Indented);
            var httpContent = new StringContent(serializedClient, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync($"/api/clients", httpContent))
            {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            using (var response = await httpClient.DeleteAsync($"/api/clients/{id}"))
            {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Profile(int id)
        {
            ClientProfileViewModel client;

            using (var response = await httpClient.GetAsync($"/api/clients/{id}/profile"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<ClientProfileViewModel>(apiResponse);
            }

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
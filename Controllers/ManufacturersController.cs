using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCRM_Online.Db;
using MyCRM_Online.Models.Entities;
using MyCRM_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyCRM_Online.ViewModels.Manufacturers;
using MyCRM_Online.ViewModels.ExchangeRates;
using Newtonsoft.Json;
using System.Net.Http;
using MyCRM_Online.Extensions;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class ManufacturersController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;

        public ManufacturersController(DataContext dataContext, IMapper mapper, IHttpClientFactory factory)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.httpClient = factory.CreateClient("apiClient");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageInfo = new PageInfo<ManufacturerEntity>();

            using (var response = await httpClient.GetAsync($"/api/manufacturers?page={page}"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                pageInfo = JsonConvert.DeserializeObject<PageInfo<ManufacturerEntity>>(apiResponse);
            }

            return View(pageInfo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ManufacturerCreateViewModel manufacturer)
        {
            if (!ModelState.IsValid)
            {
                return View(manufacturer);                
            }

            var serializedManufacturer = JsonConvert.SerializeObject(manufacturer, Formatting.Indented);
            var httpContent = new StringContent(serializedManufacturer, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync($"/api/manufacturers", httpContent))
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

            ManufacturerEditViewModel manufacturer;

            using (var response = await httpClient.GetAsync($"/api/manufacturers/{id}"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                manufacturer = JsonConvert.DeserializeObject<ManufacturerEditViewModel>(apiResponse);
            }

            return View(manufacturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, ManufacturerEditViewModel manufacturer)
        {
            if (manufacturer.Id != id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                return View(manufacturer);
            }

            var serializedManufacturer = JsonConvert.SerializeObject(manufacturer, Formatting.Indented);
            var httpContent = new StringContent(serializedManufacturer, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync($"/api/manufacturers", httpContent))
            {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int? id)
        {
            if (id == null || id < 1) {
                return NotFound();
            }

            using (var response = await httpClient.DeleteAsync($"/api/manufacturers/{id}")) {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }
    }
}

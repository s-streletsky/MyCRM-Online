using AutoMapper;
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
using MyCRM_Online.ViewModels.StockArrivals;
using Microsoft.AspNetCore.Authorization;
using MyCRM_Online.ViewModels.StockItems;
using Newtonsoft.Json;
using System.Net.Http;
using MyCRM_Online.Extensions;
using MyCRM_Online.ViewModels.Clients;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class StockArrivalsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly HttpClient httpClient;

        public StockArrivalsController(DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider, IHttpClientFactory factory)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
            this.httpClient = factory.CreateClient("apiClient");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageInfo = new PageInfo<StockArrivalViewModel>();

            using (var response = await httpClient.GetAsync($"/api/stockarrivals?page={page}")) {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                pageInfo = JsonConvert.DeserializeObject<PageInfo<StockArrivalViewModel>>(apiResponse);
            }

            return View(pageInfo);
        }

        public async Task<IActionResult> Create()
        {
            await SetAllStockItemsListToViewBagAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] StockArrivalCreateViewModel stockArrival)
        {
            if (!ModelState.IsValid) {
                await SetAllStockItemsListToViewBagAsync();

                return View(stockArrival);
            }

            var serializedStockArrival = JsonConvert.SerializeObject(stockArrival, Formatting.Indented);
            var httpContent = new StringContent(serializedStockArrival, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync($"/api/stockarrivals", httpContent)) {
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

            StockArrivalEditViewModel stockArrival;

            using (var response = await httpClient.GetAsync($"/api/stockarrivals/{id}"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                stockArrival = JsonConvert.DeserializeObject<StockArrivalEditViewModel>(apiResponse);
            }

            await SetAllStockItemsListToViewBagAsync();

            return View(stockArrival);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, StockArrivalEditViewModel stockArrival)
        {
            if (stockArrival.Id != id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                await SetAllStockItemsListToViewBagAsync();

                return View(stockArrival);
            }

            var serializedStockArrival = JsonConvert.SerializeObject(stockArrival, Formatting.Indented);
            var httpContent = new StringContent(serializedStockArrival, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync($"/api/stockarrivals", httpContent)) {
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

            using (var response = await httpClient.DeleteAsync($"/api/stockarrivals/{id}")) {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }

        private async Task SetAllStockItemsListToViewBagAsync()
        {
            IEnumerable<StockItemEntity> stockItems;

            using (var response = await httpClient.GetAsync($"/api/stockitems/list")) {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                stockItems = JsonConvert.DeserializeObject<IEnumerable<StockItemEntity>>(apiResponse);
            }

            ViewBag.StockItems = stockItems;
        }
    }
}

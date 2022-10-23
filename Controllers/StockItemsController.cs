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
using MyCRM_Online.Processors;
using MyCRM_Online.ViewModels.StockItems;
using MyCRM_Online.ViewModels.Clients;
using Newtonsoft.Json;
using System.Net.Http;
using MyCRM_Online.Extensions;
using MyCRM_Online.ViewModels.Currencies;
using MyCRM_Online.ViewModels.Manufacturers;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class StockItemsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;

        public StockItemsController(DataContext dataContext, IMapper mapper, IHttpClientFactory factory)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.httpClient = factory.CreateClient("apiClient");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageInfo = new PageInfo<StockItemViewModel>();

            using (var response = await httpClient.GetAsync($"/api/stockitems?page={page}"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                pageInfo = JsonConvert.DeserializeObject<PageInfo<StockItemViewModel>>(apiResponse);
            }

            return View(pageInfo);
        }

        public async Task<IActionResult> Create()
        {
            await SetAllManufacturersListToViewBagAsync();
            await SetAllCurrenciesListToViewBagAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] StockItemCreateViewModel stockItem)
        {
            if (!ModelState.IsValid) {
                await SetAllManufacturersListToViewBagAsync();
                await SetAllCurrenciesListToViewBagAsync();

                return View(stockItem);
            }

            var serializedStockItem = JsonConvert.SerializeObject(stockItem, Formatting.Indented);
            var httpContent = new StringContent(serializedStockItem, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync($"/api/stockitems", httpContent)) {
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

            StockItemEditViewModel stockItem;

            using (var response = await httpClient.GetAsync($"/api/stockitems/{id}")) {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                stockItem = JsonConvert.DeserializeObject<StockItemEditViewModel>(apiResponse);
            }

            await SetAllManufacturersListToViewBagAsync();
            await SetAllCurrenciesListToViewBagAsync();            

            return View(stockItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, StockItemEditViewModel stockItem)
        {
            if (stockItem.Id != id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                return View(stockItem);
            }

            var serializedStockItem = JsonConvert.SerializeObject(stockItem, Formatting.Indented);
            var httpContent = new StringContent(serializedStockItem, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync($"/api/stockitems", httpContent)) {
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

            using (var response = await httpClient.DeleteAsync($"/api/stockitems/{id}")) {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }

        private async Task SetAllManufacturersListToViewBagAsync()
        {
            IEnumerable<ManufacturerEntity> manufacturers;

            using (var response = await httpClient.GetAsync($"/api/manufacturers/list"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                manufacturers = JsonConvert.DeserializeObject<IEnumerable<ManufacturerEntity>>(apiResponse);
            }

            ViewBag.Manufacturers = manufacturers;
        }

        private async Task SetAllCurrenciesListToViewBagAsync()
        {
            IEnumerable<CurrencyViewModel> currencies;

            using (var response = await httpClient.GetAsync($"/api/currencies/list"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                currencies = JsonConvert.DeserializeObject<IEnumerable<CurrencyViewModel>>(apiResponse);
            }

            ViewBag.Currencies = currencies;
        }
    }
}

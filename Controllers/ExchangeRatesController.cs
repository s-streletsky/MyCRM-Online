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
using MyCRM_Online.ViewModels.ExchangeRates;
using Microsoft.AspNetCore.Authorization;
using MyCRM_Online.ViewModels.Clients;
using Newtonsoft.Json;
using System.Net.Http;
using MyCRM_Online.Extensions;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class ExchangeRatesController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly HttpClient httpClient;

        public ExchangeRatesController(DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider, IHttpClientFactory factory)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
            this.httpClient = factory.CreateClient("apiClient");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageInfo = new PageInfo<ExchangeRateViewModel>();

            using (var response = await httpClient.GetAsync($"/api/exchangerates?page={page}"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                pageInfo = JsonConvert.DeserializeObject<PageInfo<ExchangeRateViewModel>>(apiResponse);
            }

            return View(pageInfo);
        }

        public IActionResult Create()
        {
            SetAllCurrenciesListToViewBag();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ExchangeRateCreateViewModel exchangeRate)
        {
            if (!ModelState.IsValid)
            {
                SetAllCurrenciesListToViewBag();

                return View(exchangeRate);
            }

            var serializedExchangeRate = JsonConvert.SerializeObject(exchangeRate, Formatting.Indented);
            var httpContent = new StringContent(serializedExchangeRate, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync($"/api/exchangerates", httpContent))
            {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit([FromRoute] int? id)
        {
            if (id == null || id < 1) {
                return NotFound();
            }

            ExchangeRateEditViewModel exchangeRate;

            using (var response = await httpClient.GetAsync($"/api/exchangerates/{id}"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                exchangeRate = JsonConvert.DeserializeObject<ExchangeRateEditViewModel>(apiResponse);
            }

            SetAllCurrenciesListToViewBag();                                    
            
            return View(exchangeRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, ExchangeRateEditViewModel exchangeRate)
        {
            if (exchangeRate.Id != id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                SetAllCurrenciesListToViewBag();

                return View(exchangeRate);                
            }

            var serializedExchangeRate = JsonConvert.SerializeObject(exchangeRate, Formatting.Indented);
            var httpContent = new StringContent(serializedExchangeRate, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync($"/api/exchangerates", httpContent))
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

            using (var response = await httpClient.DeleteAsync($"/api/exchangerates/{id}"))
            {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }

        private void SetAllCurrenciesListToViewBag()
        {
            var currencies = dataContext.Currencies.ToList();
            ViewBag.Currencies = currencies;
        }
    }
}

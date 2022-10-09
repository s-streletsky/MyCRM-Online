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

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class ExchangeRatesController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public ExchangeRatesController(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            IQueryable<ExchangeRateEntity> source = dataContext.ExchangeRates;
            var totalCount = await source.CountAsync();
            var entities = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var exchangeRates = mapper.Map<List<ExchangeRateViewModel>>(entities);

            var pageInfo = new PageInfo<ExchangeRateViewModel>(totalCount, page, pageSize, exchangeRates);

            return View(pageInfo);
        }

        public IActionResult Create()
        {
            SetAllCurrenciesListToViewBag();

            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] ExchangeRateCreateViewModel exchangeRate)
        {
            var newExchangeRate = mapper.Map<ExchangeRateEntity>(exchangeRate);
            newExchangeRate.Date = DateTime.UtcNow;
            dataContext.ExchangeRates.Add(newExchangeRate);
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit([FromQuery] int? exchangeRateId)
        {
            SetAllCurrenciesListToViewBag();

            if (exchangeRateId == null || exchangeRateId == 0)
            {
                return NotFound();
            }
            var source = dataContext.ExchangeRates.Find(exchangeRateId);
            var exchangeRate = mapper.Map<ExchangeRateEditViewModel>(source);

            if (exchangeRate == null)
            {
                return NotFound();
            }
            return View(exchangeRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ExchangeRateEditViewModel ExchangeRate)
        {
            if (ModelState.IsValid)
            {
                var entity = dataContext.ExchangeRates.Single<ExchangeRateEntity>(m => m.Id == ExchangeRate.Id);
                mapper.Map(ExchangeRate, entity);
                dataContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete([FromForm] int? id)
        {
            dataContext.ExchangeRates.Remove(new ExchangeRateEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private void SetAllCurrenciesListToViewBag()
        {
            var currencies = dataContext.Currencies.ToList();
            ViewBag.Currencies = currencies;
        }
    }
}

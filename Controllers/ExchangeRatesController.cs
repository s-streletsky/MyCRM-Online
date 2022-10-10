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
        private readonly IDateTimeProvider dateTimeProvider;

        public ExchangeRatesController(DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
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
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] ExchangeRateCreateViewModel exchangeRate)
        {
            if (!ModelState.IsValid)
            {
                SetAllCurrenciesListToViewBag();

                return View(exchangeRate);
            }

            var newExchangeRate = mapper.Map<ExchangeRateEntity>(exchangeRate);
            newExchangeRate.Date = dateTimeProvider.UtcNow;
            dataContext.ExchangeRates.Add(newExchangeRate);
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit([FromRoute] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var entity = dataContext.ExchangeRates.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            var exchangeRate = mapper.Map<ExchangeRateEditViewModel>(entity);

            SetAllCurrenciesListToViewBag();                                    
            
            return View(exchangeRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, ExchangeRateEditViewModel exchangeRate)
        {
            if (exchangeRate.Id != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                SetAllCurrenciesListToViewBag();

                return View(exchangeRate);                
            }

            var entity = dataContext.ExchangeRates.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            mapper.Map(exchangeRate, entity);
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete([FromForm] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

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

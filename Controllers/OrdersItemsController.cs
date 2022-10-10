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
using MyCRM_Online.Processors;
using MyCRM_Online.ViewModels.OrdersItems;
using Microsoft.AspNetCore.Authorization;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class OrdersItemsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public OrdersItemsController(DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            IQueryable<OrderItemEntity> source = dataContext.OrdersItems;
            var totalCount = await source.CountAsync();
            var entities = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var orderItems = mapper.Map<List<OrderItemViewModel>>(entities);

            var pageInfo = new PageInfo<OrderItemViewModel>(totalCount, page, pageSize, orderItems);

            return View(pageInfo);
        }

        public IActionResult Create([FromRoute] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var orderItem = new OrderItemCreateViewModel() { OrderId = id };
            ViewBag.StockItems = dataContext.StockItems.ToList();            

            return View(orderItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] OrderItemCreateViewModel orderItem)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StockItems = dataContext.StockItems.ToList();

                return View(orderItem);
            }

            var exchangeRates = GetCurrentExchangeRates();

            var entity = mapper.Map<OrderItemEntity>(orderItem);                                   
            entity.StockItem = dataContext.StockItems.Find(orderItem.StockItemId);

            var orderItemStateProcessor = new OrderItemStateProcessor(exchangeRates);
            orderItemStateProcessor.Calculate(entity);

            dataContext.OrdersItems.Add(entity);
            dataContext.SaveChanges();

            return RedirectToAction("Edit", "Orders", new { id = entity.OrderId });
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var entity = dataContext.OrdersItems.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            ViewBag.StockItems = dataContext.StockItems.ToList();
                     
            var orderItem = mapper.Map<OrderItemEditViewModel>(entity);

            return View(orderItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, OrderItemEditViewModel orderItem)
        {
            if (orderItem.Id != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.StockItems = dataContext.StockItems.ToList();

                return View(orderItem);                
            }

            var entity = dataContext.OrdersItems.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            var exchangeRates = GetCurrentExchangeRates();

            mapper.Map(orderItem, entity);

            var orderItemStateProcessor = new OrderItemStateProcessor(exchangeRates);
            orderItemStateProcessor.Calculate(entity);

            dataContext.SaveChanges();

            return RedirectToAction("Edit", "Orders", new { id = entity.OrderId });
        }

        [HttpPost]
        public IActionResult Delete([FromForm] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            dataContext.OrdersItems.Remove(new OrderItemEntity() { Id = id });
            dataContext.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        private IEnumerable<ExchangeRateEntity> GetCurrentExchangeRates()
        {
            var currencies = dataContext.Currencies.ToList();
            var exchangeRates = new List<ExchangeRateEntity>();

            foreach (var currency in currencies)
            {
                var currentExchangeRate = dataContext.ExchangeRates
                    .Where(e => e.CurrencyId == currency.Id)
                    .OrderByDescending(e => e.Id)
                    .FirstOrDefault();

                if (currentExchangeRate != null)
                    exchangeRates.Add(currentExchangeRate);

                else
                {
                    var defaultExchangeRate = new ExchangeRateEntity()
                    {
                        Id = -1,
                        Date = dateTimeProvider.UtcNow,
                        CurrencyId = currency.Id,
                        Value = 1
                    };

                    exchangeRates.Add(defaultExchangeRate);
                }                
            }

            return exchangeRates;
        }
    }
}

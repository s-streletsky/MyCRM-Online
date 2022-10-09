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

        public OrdersItemsController(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
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

        public IActionResult Create([FromQuery] int orderId)
        {
            ViewBag.StockItems = dataContext.StockItems.ToList();
            var orderItem = new OrderItemCreateViewModel() { OrderId = orderId };

            return View(orderItem);
        }

        [HttpPost]
        public IActionResult Create([FromForm] OrderItemCreateViewModel orderItem)
        {
            var exchangeRates = GetCurrentExchangeRates();

            var entity = mapper.Map<OrderItemEntity>(orderItem);
            entity.StockItem = dataContext.StockItems.Find(orderItem.StockItemId);

            var orderItemStateProcessor = new OrderItemStateProcessor(exchangeRates);
            orderItemStateProcessor.Calculate(entity);

            dataContext.OrdersItems.Add(entity);
            dataContext.SaveChanges();

            return RedirectToAction("Edit", "Orders", new { id = entity.OrderId });
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.StockItems = dataContext.StockItems.ToList();

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var source = dataContext.OrdersItems.Find(id);
            var orderItem = mapper.Map<OrderItemEditViewModel>(source);

            if (orderItem == null)
            {
                return NotFound();
            }
            return View(orderItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderItemEditViewModel orderItem)
        {
            if (ModelState.IsValid)
            {
                var exchangeRates = GetCurrentExchangeRates();

                var entity = dataContext.OrdersItems.Single<OrderItemEntity>(c => c.Id == orderItem.Id);
                mapper.Map(orderItem, entity);

                var orderItemStateProcessor = new OrderItemStateProcessor(exchangeRates);
                orderItemStateProcessor.Calculate(entity);

                dataContext.SaveChanges();

                return RedirectToAction("Edit", "Orders", new { id = entity.OrderId });
            }

            return RedirectToAction("Edit", "Orders", new { id = orderItem.OrderId });
        }

        [HttpPost]
        public IActionResult Delete([FromForm] int id)
        {
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
                        Date = DateTime.UtcNow,
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

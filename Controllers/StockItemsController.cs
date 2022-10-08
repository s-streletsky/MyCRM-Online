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

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class StockItemsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public StockItemsController(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            IQueryable<StockItemEntity> source = dataContext.StockItems;
            var totalCount = await source.CountAsync();
            var entities = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var mappedEntities = mapper.Map<List<StockItemViewModel>>(entities);

            var quantityProcessor = new StockItemsQuantityProcessor(dataContext);
            quantityProcessor.GetQuantity(mappedEntities);

            var pageInfo = new PageInfo<StockItemViewModel>(totalCount, page, pageSize, mappedEntities);

            return View(pageInfo);
        }

        public IActionResult Create()
        {
            SetAllManufacturersListToViewBag();
            SetAllCurrenciesListToViewBag();

            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] StockItemCreateViewModel stockItem)
        {
            if (ModelState.IsValid)
            {
                var newStockItem = mapper.Map<StockItemEntity>(stockItem);
                dataContext.StockItems.Add(newStockItem);
                dataContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            SetAllManufacturersListToViewBag();
            SetAllCurrenciesListToViewBag();

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var source = dataContext.StockItems.Find(id);
            var stockItem = mapper.Map<StockItemEditViewModel>(source);

            if (stockItem == null)
            {
                return NotFound();
            }
            return View(stockItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StockItemEditViewModel stockItem)
        {
            if (ModelState.IsValid)
            {
                var entity = dataContext.StockItems.Single<StockItemEntity>(s => s.Id == stockItem.Id);
                mapper.Map(stockItem, entity);
                dataContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete([FromForm] int? id)
        {
            dataContext.StockItems.Remove(new StockItemEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private void SetAllManufacturersListToViewBag()
        {
            var manufacturers = dataContext.Manufacturers.ToList();
            ViewBag.Manufacturers = manufacturers;
        }

        private void SetAllCurrenciesListToViewBag()
        {
            var currencies = dataContext.Currencies.ToList();
            ViewBag.Currencies = currencies;
        }
    }
}

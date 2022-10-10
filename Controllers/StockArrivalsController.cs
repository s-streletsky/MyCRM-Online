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

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class StockArrivalsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public StockArrivalsController(DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            IQueryable<StockArrivalEntity> source = dataContext.StockArrivals;
            var totalCount = await source.CountAsync();
            var entities = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var stockArrivals = mapper.Map<List<StockArrivalViewModel>>(entities);

            var pageInfo = new PageInfo<StockArrivalViewModel>(totalCount, page, pageSize, stockArrivals);

            return View(pageInfo);
        }

        public IActionResult Create()
        {
            SetAllStockItemsListToViewBag();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] StockArrivalCreateViewModel stockArrival)
        {
            if (!ModelState.IsValid)
            {
                SetAllStockItemsListToViewBag();

                return View(stockArrival);
            }

            var newStockArrival = mapper.Map<StockArrivalEntity>(stockArrival);
            newStockArrival.Date = dateTimeProvider.UtcNow;
            dataContext.StockArrivals.Add(newStockArrival);
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var entity = dataContext.StockArrivals.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            SetAllStockItemsListToViewBag();

            var stockArrival = mapper.Map<StockArrivalEditViewModel>(entity);

            return View(stockArrival);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, StockArrivalEditViewModel stockArrival)
        {
            if (stockArrival.Id != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                SetAllStockItemsListToViewBag();

                return View(stockArrival);
            }

            var entity = dataContext.StockArrivals.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            mapper.Map(stockArrival, entity);
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

            dataContext.StockArrivals.Remove(new StockArrivalEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private void SetAllStockItemsListToViewBag()
        {
            var stockItems = dataContext.StockItems.ToList();
            ViewBag.StockItems = stockItems;
        }
    }
}

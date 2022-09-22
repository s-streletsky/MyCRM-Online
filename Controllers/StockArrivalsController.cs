using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyCRM_Online.Db;
using MyCRM_Online.Models.Entities;
using MyCRM_Online.Models;
using MyCRM_Online.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyCRM_Online.Controllers
{
    public class StockArrivalsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public StockArrivalsController(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
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
            GetStockItems();

            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] StockArrivalCreateViewModel stockArrival)
        {
            var newStockArrival = mapper.Map<StockArrivalEntity>(stockArrival);
            newStockArrival.Date = DateTime.UtcNow;
            dataContext.StockArrivals.Add(newStockArrival);
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            GetStockItems();

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var source = dataContext.StockArrivals.Find(id);
            var stockArrival = mapper.Map<StockArrivalEditViewModel>(source);

            if (stockArrival == null)
            {
                return NotFound();
            }
            return View(stockArrival);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StockArrivalEditViewModel stockArrival)
        {
            if (ModelState.IsValid)
            {
                var entity = dataContext.StockArrivals.Single<StockArrivalEntity>(s => s.Id == stockArrival.Id);
                mapper.Map(stockArrival, entity);
                dataContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete([FromForm] int? id)
        {
            dataContext.StockArrivals.Remove(new StockArrivalEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private void GetStockItems()
        {
            var stockItems = dataContext.StockItems.ToList();
            ViewBag.StockItems = stockItems;
        }
    }
}

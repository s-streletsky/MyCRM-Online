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
using MyCRM_Online.ViewModels.Orders;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public OrdersController(DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            IQueryable<OrderEntity> source = dataContext.Orders;
            var totalCount = await source.CountAsync();
            var entities = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var orders = mapper.Map<List<OrderViewModel>>(entities);

            var pageInfo = new PageInfo<OrderViewModel>(totalCount, page, pageSize, orders);

            return View(pageInfo);
        }        

        public IActionResult Create()
        {
            SetAllClientsListToViewBag();
            SetOrderStatusesListToViewBag();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] OrderCreateViewModel order)
        {
            if (!ModelState.IsValid)
            {
                SetAllClientsListToViewBag();
                SetOrderStatusesListToViewBag();

                return View(order);
            }

            var newOrder = mapper.Map<OrderEntity>(order);
            newOrder.Date = dateTimeProvider.UtcNow;
            newOrder.StatusId = 4;
            dataContext.Orders.Add(newOrder);
            dataContext.SaveChanges();

            return RedirectToAction("Edit", new { id = newOrder.Id });
        }

        public IActionResult CreateFast([FromRoute] int id)
        {
            var order = new OrderCreateViewModel() { ClientId = id };
            var newOrder = mapper.Map<OrderEntity>(order);
            newOrder.Date = DateTime.UtcNow;
            newOrder.StatusId = 4;
            dataContext.Orders.Add(newOrder);
            dataContext.SaveChanges();

            TempData["OrderId"] = newOrder.Id;

            return RedirectToAction("Edit", new { id = newOrder.Id });
        }

        public IActionResult Edit([FromRoute] int? id)
        {
            if (id == null || id == 0)
            {
                id = (int)TempData["OrderId"];
            }

            var entity = dataContext.Orders.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            var order = mapper.Map<OrderEditViewModel>(entity);

            SetOrderStatusesListToViewBag();
            
            ViewBag.OrderItems = dataContext.OrdersItems.Where(i => i.OrderId == id).ToList();
            ViewBag.OrderTotal = dataContext.OrdersItems.Where(i => i.OrderId == id).Sum(i => i.Total);
            ViewBag.PaymentsTotal = dataContext.Payments.Where(p => p.OrderId == id).Sum(p => p.Amount);
            ViewBag.Debt = ViewBag.OrderTotal - ViewBag.PaymentsTotal;
            
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, OrderEditViewModel order)
        {
            if (order.Id != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                SetOrderStatusesListToViewBag();

                return View(order);                
            }

            var entity = dataContext.Orders.Find(order.Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.StatusId = order.StatusId;
            entity.Notes = order.Notes;
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

            dataContext.Orders.Remove(new OrderEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private void SetAllClientsListToViewBag()
        {
            var clients = dataContext.Clients.ToList();
            ViewBag.Clients = clients;
        }

        private void SetOrderStatusesListToViewBag()
        {
            var statuses = dataContext.OrderStatuses.ToList();
            ViewBag.Statuses = statuses;
        }
    }
}

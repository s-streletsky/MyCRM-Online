using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public OrdersController(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
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
            GetClients();
            GetOrderStatuses();

            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] OrderCreateViewModel order)
        {
            var newOrder = mapper.Map<OrderEntity>(order);
            newOrder.Date = DateTime.UtcNow;
            newOrder.StatusId = 4;
            dataContext.Orders.Add(newOrder);
            dataContext.SaveChanges();

            //TempData["OrderId"] = newOrder.Id;

            return RedirectToAction("Edit", new { id = newOrder.Id });
        }

        public IActionResult FastCreate([FromRoute] int id)
        {
            var order = new OrderCreateViewModel() { ClientId = id };
            var newOrder = mapper.Map<OrderEntity>(order);
            newOrder.Date = DateTime.UtcNow;
            newOrder.StatusId = 4;
            dataContext.Orders.Add(newOrder);
            dataContext.SaveChanges();

            TempData["OrderId"] = newOrder.Id;

            return RedirectToAction("Edit");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                id = (int)TempData["OrderId"];
            }

            GetOrderStatuses();

            var source = dataContext.Orders.Find(id);
            var order = mapper.Map<OrderEditViewModel>(source);
            ViewBag.OrderItems = dataContext.OrdersItems.Where(i => i.OrderId == id).ToList();
            ViewBag.OrderTotal = dataContext.OrdersItems.Where(i => i.OrderId == id).Sum(i => i.Total);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm]OrderEditViewModel order)
        {
            if (ModelState.IsValid)
            {
                var entity = dataContext.Orders.Find(order.Id);
                mapper.Map(order, entity);
                dataContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete([FromForm] int? id)
        {
            dataContext.Orders.Remove(new OrderEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private void GetClients()
        {
            var clients = dataContext.Clients.ToList();
            ViewBag.Clients = clients;
        }

        private void GetOrderStatuses()
        {
            var statuses = dataContext.OrderStatuses.ToList();
            ViewBag.Statuses = statuses;
        }
    }
}

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
using MyCRM_Online.ViewModels.Payments;
using Microsoft.AspNetCore.Authorization;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public PaymentsController(DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            IQueryable<PaymentEntity> source = dataContext.Payments;
            var totalCount = await source.CountAsync();
            var entities = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pageInfo = new PageInfo<PaymentEntity>(totalCount, page, pageSize, entities);

            return View(pageInfo);
        }

        public IActionResult CreateFast([FromRoute] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var orderEntity = dataContext.Orders.Find(id);

            if (orderEntity == null)
            {
                return NotFound();
            }

            var newPayment = new PaymentCreateViewModel();

            newPayment.ClientId = orderEntity.ClientId;
            newPayment.OrderId = orderEntity.Id;

            return View(newPayment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateFast([FromForm] PaymentCreateViewModel payment)
        {
            if (!ModelState.IsValid)
            {
                return View(payment);
            }

            var newPayment = mapper.Map<PaymentEntity>(payment);
            newPayment.Date = dateTimeProvider.UtcNow;
            dataContext.Payments.Add(newPayment);
            dataContext.SaveChanges();

            return RedirectToAction("Edit", "Orders", new { id = newPayment.OrderId });
        }

        public IActionResult Edit([FromRoute] int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var entity = dataContext.Payments.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            var payment = mapper.Map<PaymentEditViewModel>(entity);
           
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, PaymentEditViewModel payment)
        {
            if (payment.Id != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(payment);                
            }

            var entity = dataContext.Payments.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            mapper.Map(payment, entity);
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

            dataContext.Payments.Remove(new PaymentEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

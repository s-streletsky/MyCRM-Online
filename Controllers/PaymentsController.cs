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

namespace MyCRM_Online.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public PaymentsController(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
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

        public IActionResult CreateFast(int id)
        {
            var order = dataContext.Orders.Find(id);
            var newPayment = new PaymentCreateViewModel();

            newPayment.ClientId = order.ClientId;
            newPayment.OrderId = order.Id;

            return View(newPayment);
        }

        [HttpPost]
        public IActionResult CreateFast([FromForm] PaymentCreateViewModel payment)
        {
            var newPayment = mapper.Map<PaymentEntity>(payment);
            newPayment.Date = DateTime.UtcNow;
            dataContext.Payments.Add(newPayment);
            dataContext.SaveChanges();

            return RedirectToAction("Edit", "Orders", new { id = newPayment.OrderId });
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var source = dataContext.Payments.Find(id);
            var payment = mapper.Map<PaymentEditViewModel>(source);

            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PaymentEditViewModel payment)
        {
            if (ModelState.IsValid)
            {
                var entity = dataContext.Payments.Single<PaymentEntity>(p => p.Id == payment.Id);
                mapper.Map(payment, entity);
                dataContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete([FromForm] int? id)
        {
            dataContext.Payments.Remove(new PaymentEntity() { Id = id });
            dataContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

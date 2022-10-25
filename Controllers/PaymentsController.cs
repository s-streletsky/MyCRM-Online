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
using MyCRM_Online.ViewModels.Clients;
using Newtonsoft.Json;
using System.Net.Http;
using MyCRM_Online.Extensions;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly HttpClient httpClient;

        public PaymentsController(DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider, IHttpClientFactory factory)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
            this.httpClient = factory.CreateClient("apiClient");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageInfo = new PageInfo<PaymentViewModel>();

            using (var response = await httpClient.GetAsync($"/api/payments?page={page}")) {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                pageInfo = JsonConvert.DeserializeObject<PageInfo<PaymentViewModel>>(apiResponse);
            }

            return View(pageInfo);
        }

        public async Task<IActionResult> CreateFast([FromRoute] int? id)
        {
            if (id == null || id < 1) {
                return NotFound();
            }

            OrderEntity order;

            using (var response = await httpClient.GetAsync($"/api/orders/{id}")) {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                order = JsonConvert.DeserializeObject<OrderEntity>(apiResponse);
            }

            var newPayment = new PaymentCreateViewModel();

            newPayment.ClientId = order.ClientId;
            newPayment.OrderId = order.Id;

            return View(newPayment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFast([FromForm] PaymentCreateViewModel payment)
        {
            if (!ModelState.IsValid) {
                return View(payment);
            }

            var serializedPayment = JsonConvert.SerializeObject(payment, Formatting.Indented);
            var httpContent = new StringContent(serializedPayment, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync($"/api/payments", httpContent)) {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Edit", "Orders", new { id = payment.OrderId });
        }

        public async Task<IActionResult> Edit([FromRoute] int? id)
        {
            if (id == null || id < 1) {
                return NotFound();
            }

            PaymentEditViewModel payment;

            using (var response = await httpClient.GetAsync($"/api/payments/{id}")) {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                payment = JsonConvert.DeserializeObject<PaymentEditViewModel>(apiResponse);
            }

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, PaymentEditViewModel payment)
        {
            if (payment.Id != id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                return View(payment);                
            }

            var serializedPayment = JsonConvert.SerializeObject(payment, Formatting.Indented);
            var httpContent = new StringContent(serializedPayment, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync($"/api/payments", httpContent)) {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int? id)
        {
            if (id == null || id < 1) {
                return NotFound();
            }

            using (var response = await httpClient.DeleteAsync($"/api/payments/{id}")) {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Index");
        }
    }
}

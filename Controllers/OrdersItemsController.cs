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
using MyCRM_Online.ViewModels.Clients;
using Newtonsoft.Json;
using System.Net.Http;
using MyCRM_Online.Extensions;
using MyCRM_Online.ViewModels.Currencies;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyCRM_Online.Controllers
{
    [Authorize]
    public class OrdersItemsController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly HttpClient httpClient;

        public OrdersItemsController(DataContext dataContext, IMapper mapper, IDateTimeProvider dateTimeProvider, IHttpClientFactory factory)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
            this.httpClient = factory.CreateClient("apiClient");
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageInfo = new PageInfo<OrderItemViewModel>();

            using (var response = await httpClient.GetAsync($"/api/ordersitems?page={page}")) {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                pageInfo = JsonConvert.DeserializeObject<PageInfo<OrderItemViewModel>>(apiResponse);
            }

            return View(pageInfo);
        }

        public async Task<IActionResult> Create([FromRoute] int? id)
        {
            if (id == null || id < 1) {
                return NotFound();
            }

            var orderItem = new OrderItemCreateViewModel() { OrderId = id };
            await SetAllStockItemsListToViewBagAsync();

            return View(orderItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] OrderItemCreateViewModel orderItem)
        {
            if (!ModelState.IsValid) {
                await SetAllStockItemsListToViewBagAsync();

                return View(orderItem);
            }

            var serializedOrderItem = JsonConvert.SerializeObject(orderItem, Formatting.Indented);
            var httpContent = new StringContent(serializedOrderItem, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync($"/api/ordersitems", httpContent)) {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Edit", "Orders", new { id = orderItem.OrderId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int? id)
        {
            if (id == null || id < 1) {
                return NotFound();
            }

            OrderItemEditViewModel orderItem;

            using (var response = await httpClient.GetAsync($"/api/ordersitems/{id}"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                orderItem = JsonConvert.DeserializeObject<OrderItemEditViewModel>(apiResponse);
            }

            await SetAllStockItemsListToViewBagAsync();                    

            return View(orderItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, OrderItemEditViewModel orderItem)
        {
            if (orderItem.Id != id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                await SetAllStockItemsListToViewBagAsync();

                return View(orderItem);                
            }

            var serializedOrderItem = JsonConvert.SerializeObject(orderItem, Formatting.Indented);
            var httpContent = new StringContent(serializedOrderItem, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync($"/api/ordersitems", httpContent)) {
                response.ThrowOnHttpError();
            }

            return RedirectToAction("Edit", "Orders", new { id = orderItem.OrderId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int? id)
        {
            if (id == null || id < 1) {
                return NotFound();
            }

            using (var response = await httpClient.DeleteAsync($"/api/ordersitems/{id}")) {
                response.ThrowOnHttpError();
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }     

        private async Task SetAllStockItemsListToViewBagAsync()
        {
            IEnumerable<StockItemEntity> stockItems;

            using (var response = await httpClient.GetAsync($"/api/stockitems/list"))
            {
                response.ThrowOnHttpError();

                var apiResponse = await response.Content.ReadAsStringAsync();
                stockItems = JsonConvert.DeserializeObject<IEnumerable<StockItemEntity>>(apiResponse);
            }

            ViewBag.StockItems = stockItems;
        }
    }
}

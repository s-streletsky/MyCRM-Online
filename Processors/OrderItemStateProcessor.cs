using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.Processors
{
    public class OrderItemStateProcessor
    {
        private readonly IEnumerable<ExchangeRateEntity> exchangeRates;

        public OrderItemStateProcessor(IEnumerable<ExchangeRateEntity> exchangeRates)
        {
            this.exchangeRates = exchangeRates;
        }

        public void Calculate(OrderItemEntity orderItem)
        {
            foreach (var exchangeRate in exchangeRates)
            {
                if (orderItem.StockItem.CurrencyId == exchangeRate.CurrencyId)
                    orderItem.ExchangeRate = exchangeRate.Value;
            }

            orderItem.Price = orderItem.StockItem.RetailPrice * orderItem.ExchangeRate;

            var discount = orderItem.Quantity * orderItem.Price * (orderItem.Discount * 0.01f);
            orderItem.Total = orderItem.Quantity * orderItem.Price - discount;

            orderItem.Expenses = orderItem.Quantity * (orderItem.StockItem.PurchasePrice * orderItem.ExchangeRate);
            orderItem.Profit = orderItem.Total - orderItem.Expenses;
        }
    }
}

using MyCRM_Online.Db;
using MyCRM_Online.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.Processors
{
    public class StockItemsQuantityProcessor
    {
        private readonly DataContext dataContext;

        public StockItemsQuantityProcessor(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void GetQuantity(IEnumerable<StockItemViewModel> stockItems)
        {
            foreach (var stockItem in stockItems)
            {
                var inOrders = dataContext.OrdersItems.Sum(i => i.Quantity);
                var inArrivals = dataContext.StockArrivals.Sum(i => i.Quantity);
                stockItem.Quantity = inArrivals - inOrders;
            }
        }
    }
}

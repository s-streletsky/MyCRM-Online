using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public StockItemEntity StockItem { get; set; }
        public OrderEntity Order { get; set; }
        public float Quantity { get; set; }
    }
}

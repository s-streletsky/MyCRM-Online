using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels
{
    public class OrderItemCreateViewModel
    {
        public int OrderId { get; set; }
        public int StockItemId { get; set; }
        public float Quantity { get; set; }
        public float Discount { get; set; }
    }
}

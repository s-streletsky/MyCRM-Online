using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.Models.Entities
{
    public class OrderItemEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int StockItemId { get; set; }
        public float Quantity { get; set; }
    }
}

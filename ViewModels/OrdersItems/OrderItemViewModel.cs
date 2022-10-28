using MyCRM_Online.Models.Entities;

namespace MyCRM_Online.ViewModels.OrdersItems
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public StockItemEntity StockItem { get; set; }
        public OrderEntity Order { get; set; }
        public float Quantity { get; set; }
    }
}

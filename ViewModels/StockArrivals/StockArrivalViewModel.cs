using MyCRM_Online.Models.Entities;

namespace MyCRM_Online.ViewModels.StockArrivals
{
    public class StockArrivalViewModel
    {
        private DateTime date;

        public int Id { get; set; }
        public DateTime Date
        {
            get { return date; }
            set { date = value.ToLocalTime(); }
        }
        public StockItemEntity StockItem { get; set; }
        public float Quantity { get; set; }
    }
}

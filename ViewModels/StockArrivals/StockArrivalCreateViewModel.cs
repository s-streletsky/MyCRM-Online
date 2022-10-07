using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels.StockArrivals
{
    public class StockArrivalCreateViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "SelectStockItem")]
        public int StockItemId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0.001, float.MaxValue, ErrorMessage = "PositiveValuesOnly")]
        public float Quantity { get; set; }
    }
}

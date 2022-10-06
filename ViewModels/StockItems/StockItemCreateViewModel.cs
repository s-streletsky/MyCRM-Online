using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels.StockItems
{
    public class StockItemCreateViewModel
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(70, MinimumLength = 2, ErrorMessage = "NameStringLenght")]
        public string Name { get; set; }
        
        public int? ManufacturerId { get; set; }

        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SelectCurrency")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0.01, float.MaxValue, ErrorMessage = "PositiveValuesOnly")]
        public float PurchasePrice { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0.01, float.MaxValue, ErrorMessage = "PositiveValuesOnly")]
        public float RetailPrice { get; set; }
    }
}

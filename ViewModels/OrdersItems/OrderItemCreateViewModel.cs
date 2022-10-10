﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels.OrdersItems
{
    public class OrderItemCreateViewModel
    {
        public int? OrderId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SelectStockItem")]
        public int StockItemId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0.001, float.MaxValue, ErrorMessage = "QuantityMinLimit")]
        public float Quantity { get; set; }

        [Range(0, 100, ErrorMessage = "DiscountRange")]
        public float Discount { get; set; }
    }
}

﻿using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels
{
    public class StockItemCreateViewModel
    {
        public string Name { get; set; }
        public int? ManufacturerId { get; set; }
        public string? Description { get; set; }
        public int CurrencyId { get; set; }
        public double PurchasePrice { get; set; }
        public double RetailPrice { get; set; }
    }
}
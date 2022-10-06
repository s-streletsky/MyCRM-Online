using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels.ExchangeRates
{
    public class ExchangeRateCreateViewModel
    {
        public int CurrencyId { get; set; }
        public double Value { get; set; }
    }
}

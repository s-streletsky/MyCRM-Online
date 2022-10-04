using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels
{
    public class PaymentCreateViewModel
    {
        public int ClientId { get; set; }
        public int? OrderId { get; set; }
        public float Amount { get; set; }
        public string? Notes { get; set; }
    }
}

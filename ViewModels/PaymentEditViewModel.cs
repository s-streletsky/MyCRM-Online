using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels
{
    public class PaymentEditViewModel
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public string? Notes { get; set; }
    }
}

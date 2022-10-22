using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels.Clients
{
    public class ClientProfileViewModel : ClientViewModel
    {
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public int OrdersQuantity { get; set; }
        public float PaymentsTotal { get; set; }
    }
}

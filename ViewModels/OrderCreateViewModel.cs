using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels
{
    public class OrderCreateViewModel
    {
        public int ClientId { get; set; }
        public string? Notes { get; set; }
    }
}

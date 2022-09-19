using MyCRM_Online.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels
{
    public class ClientsViewModel
    {
        private DateTime date;

        public int Id { get; set; }
        public DateTime Date { 
            get { return date; }
            set { date = value.ToLocalTime(); } 
        }
        public string Name { get; set; }
        public string? Nickname { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public Country? Country { get; set; }
        public string? City { get; set; }
        public ShippingMethod? ShippingMethod { get; set; }
        public string? PostalCode { get; set; }              
    }
}

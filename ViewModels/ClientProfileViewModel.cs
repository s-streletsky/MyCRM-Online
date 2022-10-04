using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels
{
    public class ClientProfileViewModel
    {
        private DateTime date;

        public int Id { get; set; }
        public DateTime Date
        {
            get { return date.ToLocalTime(); }
            set { date = value; }
        }
        public string Name { get; set; }
        public string? Nickname { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public CountryEntity? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public ShippingMethodEntity? ShippingMethod { get; set; }
        public string? PostalCode { get; set; }
        public string? Notes { get; set; }
        public int OrdersQuantity { get; set; }
        public float PaymentsTotal { get; set; }
    }
}

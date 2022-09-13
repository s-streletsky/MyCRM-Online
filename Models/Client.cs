using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace MyCRM_Online.Models
{
    public class Client
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Nickname { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }        
        public string? City { get; set; }
        public string? Address { get; set; }        
        public string? PostalCode { get; set; }
        public string? Notes { get; set; }

        public int? ShippingMethodId { get; set; }
        public ShippingMethod? ShippingMethod { get; set; }
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
    }
}

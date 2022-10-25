using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels.Payments
{
    public class PaymentCreateViewModel
    {
        public int ClientId { get; set; }
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0.01, float.MaxValue, ErrorMessage = "PositiveValuesOnly")]
        public float Amount { get; set; }

        [MaxLength(50, ErrorMessage = "StringLenghtLimit")]
        public string? Notes { get; set; }
    }
}

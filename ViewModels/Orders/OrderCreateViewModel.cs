using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels.Orders
{
    public class OrderCreateViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "SelectClient")]
        public int ClientId { get; set; }

        [MaxLength(255, ErrorMessage = "NotesLenghtLimit")]
        public string? Notes { get; set; }
    }
}

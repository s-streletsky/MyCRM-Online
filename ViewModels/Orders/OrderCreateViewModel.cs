﻿using System.ComponentModel.DataAnnotations;

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

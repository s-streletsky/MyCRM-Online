﻿using System.ComponentModel.DataAnnotations;

namespace MyCRM_Online.ViewModels.Manufacturers
{
    public class ManufacturerCreateViewModel
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "NameStringLenght")]
        public string Name { get; set; }
    }
}

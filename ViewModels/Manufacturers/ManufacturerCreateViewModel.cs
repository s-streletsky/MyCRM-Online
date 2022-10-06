using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels.Manufacturers
{
    public class ManufacturerCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}

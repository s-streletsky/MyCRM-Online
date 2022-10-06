using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels.Orders
{
    public class OrderEditViewModel : OrderCreateViewModel
    {
        private DateTime date;

        public int Id { get; set; }
        public DateTime Date
        {
            get { return date.ToLocalTime(); }
            set { date = value; }
        }
        public ClientEntity? Client { get; set; }

        public int StatusId { get; set; }
    }
}

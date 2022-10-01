﻿using MyCRM_Online.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.ViewModels
{
    public class OrderEditViewModel : OrderCreateViewModel
    {
        private DateTime date;

        public int Id { get; set; }
        public DateTime Date {
            get { return date; }
            set { date = value.ToLocalTime(); }
        }
        public ClientEntity? Client { get; set; }
        public int StatusId { get; set; }
    }
}
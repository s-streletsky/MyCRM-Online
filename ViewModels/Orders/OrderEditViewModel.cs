﻿using MyCRM_Online.Models.Entities;

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
        public IEnumerable<OrderItemEntity> Items { get; set; }
        public float Total { get; set; }
        public float PaymentsTotal { get; set; }
        public float Debt { get; set; }
    }
}

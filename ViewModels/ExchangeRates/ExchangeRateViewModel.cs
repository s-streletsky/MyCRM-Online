using MyCRM_Online.Models.Entities;

namespace MyCRM_Online.ViewModels.ExchangeRates
{
    public class ExchangeRateViewModel
    {
        private DateTime date;

        public int Id { get; set; }
        public DateTime Date
        {
            get { return date; }
            set { date = value.ToLocalTime(); }
        }
        public CurrencyEntity Currency { get; set; }
        public double Value { get; set; }
    }
}

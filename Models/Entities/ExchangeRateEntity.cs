namespace MyCRM_Online.Models.Entities
{
    public class ExchangeRateEntity
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        public int CurrencyId { get; set; }
        public CurrencyEntity Currency { get; set; }
        public float Value { get; set; }
    }
}

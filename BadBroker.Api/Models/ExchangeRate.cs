namespace BadBroker.Api.Models
{
    public class ExchangeRate
    {
        public DateTime date { get; set; }
        public double value { get; set; }
        public Currency currency { get; set; }
    }
}
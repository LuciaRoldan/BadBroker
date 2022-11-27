namespace BadBroker.Api.Models
{
    public class BestRatesResponse
    {
        public IEnumerable<RateDto> rates { get; set; }
        public DateTime buyDate { get; set; }
        public DateTime sellDate { get; set; }
        public Currency tool { get; set; }
        public double revenue { get; set; }
    }
}
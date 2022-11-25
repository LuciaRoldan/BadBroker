namespace BadBroker.Api.Models
{
    public class BestRatesResponse
    {
        public IEnumerable<Rate> rates { get; set; }
        public DateTime buyDate { get; set; }
        public DateTime sellDate { get; set; }
        public Curreny tool { get; set; }
        public double revenue { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace BadBroker.Api.Models
{
    public class BestRatesResponse
    {
        public IEnumerable<RateDto> rates { get; set; }
        public DateTime buyDate { get; set; }
        public DateTime sellDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency tool { get; set; }
        public double revenue { get; set; }
    }
}
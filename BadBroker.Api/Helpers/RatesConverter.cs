using BadBroker.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BadBroker.Api.Helpers
{
    public class RatesConverter : JsonConverter
    {
        private DateTime date;

        public RatesConverter(DateTime date)
        {
            this.date = date;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(List<ExchangeRate>) == objectType;
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JObject.Load(reader);

            ExchangeRate RUBrate = new ExchangeRate() { currency = Currency.RUB, value = token.SelectToken("rates").SelectToken("RUB").ToObject<double>(), date = date};
            ExchangeRate EURrate = new ExchangeRate() { currency = Currency.EUR, value = token.SelectToken("rates").SelectToken("EUR").ToObject<double>(), date = date};
            ExchangeRate GBPrate = new ExchangeRate() { currency = Currency.GBP, value = token.SelectToken("rates").SelectToken("GBP").ToObject<double>(), date = date};
            ExchangeRate JPYrate = new ExchangeRate() { currency = Currency.JPY, value = token.SelectToken("rates").SelectToken("JPY").ToObject<double>(), date = date};
            
            return new List<ExchangeRate>() {RUBrate, EURrate, GBPrate, JPYrate};
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
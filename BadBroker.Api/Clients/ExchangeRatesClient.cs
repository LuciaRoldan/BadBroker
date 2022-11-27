using BadBroker.Api.Helpers;
using BadBroker.Api.Models;
using Newtonsoft.Json;

namespace BadBroker.Api.Clients
{
    public interface IExchangeRatesClient
    {
        Task<List<ExchangeRate>> GetExchangeRatesFor(DateTime startDate, DateTime endDate);
    }

    public class ExchangeRatesClient : IExchangeRatesClient
    {
        private HttpClient _client;
        public string _baseUrl {get; set;} = "https://openexchangerates.org/api";
        private string _appId = "76248f4a4a374a07a56e64f2fe683b96";

        public ExchangeRatesClient()
        {
            this._client = new HttpClient();
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesFor(DateTime startDate, DateTime endDate)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            for(int i = 0; i <= (endDate - startDate).Days; i++)
            {
                rates.AddRange(await GetExchangeRatesFor(startDate.AddDays(i)));
            }
            return rates;
        }

        private async Task<List<ExchangeRate>> GetExchangeRatesFor(DateTime date)
        {
            HttpRequestMessage message = new HttpRequestMessage();

            message.Headers.Add("Authorization", "Token " + _appId);
            message.RequestUri = new Uri($"{_baseUrl}/historical/{date.ToString("yyyy-MM-dd")}.json");
            message.Method = HttpMethod.Get;

            HttpResponseMessage response = await _client.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error getting exchange rate");
            }

            var apiKeyJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ExchangeRate>>(apiKeyJson, new RatesConverter(date));
        }
    }
}
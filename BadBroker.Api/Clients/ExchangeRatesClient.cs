using BadBroker.Api.Exceptions;
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
        public string _baseUrl {get; set;}
        private string _appId = "";

        public ExchangeRatesClient(IConfiguration config)
        {
            this._baseUrl = config["ExchangeRatesConection:Url"];
            this._appId = config["ExchangeRatesConection:AppId"];
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
                throw new ConnectionErrorException();
            }

            var apiKeyJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ExchangeRate>>(apiKeyJson, new RatesConverter(date));
        }
    }
}
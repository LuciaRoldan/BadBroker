using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Matchers;

namespace BadBroker.Unit.Tests.MockServer
{
    public class ExchangeRateServerMock
    {
        private WireMockServer _server;

        public string Url
        {
            get
            {
                int port = _server.Ports[0];
                return $"http://localhost:{port}";
            }
        }
        public ExchangeRateServerMock()
        {
            _server = WireMockServer.Start();
        }

        public ExchangeRateServerMock(string url)
        {
            _server = WireMockServer.Start(url);
        }

        public void Shutdown()
        {
            var logs = _server.LogEntries;
            _server.Stop();
        }

        public void ResetExpectation()
        {
            _server.Reset();
        }

        public static ExchangeRateServerMock Start()
        {
            return new ExchangeRateServerMock();
        }


        internal void canGetExchangeRatesSuccessfullyFor(DateTime date)
        {
            _server.Given(Request.Create()
                 .WithPath("/historical/" + date.ToString("yyyy-MM-dd") + ".json")
                 .UsingGet())
            .RespondWith(Response.Create()
                 .WithStatusCode(200)
                 .WithBody(
                            "{" +
                                "\"disclaimer\": \"https://openexchangerates.org/terms/\", " +
                                "\"license\": \"https://openexchangerates.org/license/\", " +
                                "\"timestamp\": 1449877801, " +
                                "\"base\": \"USD\", " +
                                "\"rates\": { " +
                                    "\"RUB\": 1, " +
                                    "\"EUR\": 2, " +
                                    "\"GBP\": 3, " +
                                    "\"JPY\": 4, " +
                                    "\"ANG\": 1.788575, " +
                                    "\"AOA\": 135.295998, " +
                                    "\"ARS\": 9.750101, " +
                                    "\"AUD\": 1.390866, " +
                                "} " +
                            "} "                            
                            ));
        }

        internal void getExchangeRatesSuccessfullyReturnError()
        {
            _server.Given(Request.Create()
                 .UsingGet())
            .RespondWith(Response.Create()
                 .WithStatusCode(400));
        }
    }
}
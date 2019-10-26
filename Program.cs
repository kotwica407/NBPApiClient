using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NBPApiClient.ExchangeRatesReader;

namespace NBPApiClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            //TODO Chcę zrobić analizę korelacji kursów różnych walut
            //var exchangeRate = Utils.ReadExchangeRateForOneDay(Consts.DolarAmerykanski, new DateTime(2019, 10, 25));
            var exchangeRatesForPeriod = 
                Utils.ReadExchangeRatesForPeriod(Consts.DolarAmerykanski, new DateTime(2019, 10, 1), new DateTime(2019, 10, 25));
            foreach(ExchangeRate rate in exchangeRatesForPeriod.Result)
                Console.WriteLine(rate.Rate);
        }
    }
}
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NBPApiClient.ExchangeRatesReader
{
    public static class Utils
    {
        static readonly HttpClient client = new HttpClient();
        public static async ExchangeRate ReadExchangeRateForOneDay(string currency, DateTime date)
        {
            string dateString = date.ToString("yyyy-mm-dd");
            string uri = Consts.RequestUriStart + Consts.Rates + "/" + currency + "/" + dateString;
            string jsonExchangeRate = await GetJsonResponseAsync(uri);
        }

        public static async Task<string> GetJsonResponseAsync(string uri)
        {
            return await client.GetStringAsync(uri);;
        }
    }
}
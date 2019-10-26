using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NBPApiClient.ExchangeRatesReader
{
    public static class Utils
    {
        static readonly HttpClient client = new HttpClient();
        public static async Task<ExchangeRate> ReadExchangeRateForOneDay(string currency, DateTime date)
        {
            string dateString = date.ToString("yyyy-MM-dd");
            string uri = Consts.RequestUriStart + Consts.Rates 
                + Consts.TableA + currency + "/" + dateString;
            string jsonExchangeRate = await GetJsonResponseAsync(uri);
            JObject jObject = JObject.Parse(jsonExchangeRate);
            var rates = jObject["rates"].First;
            var rateString = rates["mid"].ToString();
            return new ExchangeRate
            {
                Currency = currency,
                Date = date,
                Rate = Decimal.Parse(rateString)
            };
        }

        public static async Task<IEnumerable<ExchangeRate>> ReadExchangeRatesForPeriod(string currency, DateTime firstDay, DateTime lastDay)
        {
            string firstDayString = firstDay.ToString("yyyy-MM-dd");
            string lastDayString = lastDay.ToString("yyyy-MM-dd");
            string uri = Consts.RequestUriStart + Consts.Rates 
                + Consts.TableA + currency + "/" + firstDayString + "/" + lastDayString;
            string jsonExchangeRate = await GetJsonResponseAsync(uri);
            JObject jObject = JObject.Parse(jsonExchangeRate);
            var rates = jObject["rates"];
            var result = new List<ExchangeRate>();
            foreach(JToken token in rates)
                result.Add(new ExchangeRate
                {
                    Currency = currency,
                    Date = DateTime.ParseExact(token["effectiveDate"].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                    Rate = Decimal.Parse(token["mid"].ToString()) 
                });
            return result;
        }
        private static async Task<string> GetJsonResponseAsync(string uri)
        {
            return await client.GetStringAsync(uri);;
        }
    }
}
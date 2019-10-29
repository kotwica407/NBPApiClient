using System.Collections.Generic;
using System.Linq;
using NBPApiClient.ExchangeRatesReader;
using System;

namespace NBPApiClient.TechnicalAnalysis
{
    internal static class MACD
    {
        internal static IEnumerable<ExchangeRate> CalculateMACD(IEnumerable<ExchangeRate> exchangeRates, bool longterm = false)
        {
            var lowerPeriod = longterm ? 12 : 8;
            var longerPeriod = longterm ? 26 : 17;
            var currency = exchangeRates.First().Currency;

            var exponentialAverage1 = Averages
                .ExponentialAverage(exchangeRates, lowerPeriod, 0.9);
            var exponentialAverage2 = Averages
                .ExponentialAverage(exchangeRates, longerPeriod, 0.9);

            var result = new List<ExchangeRate>();
            for(int i = 0; i < exchangeRates.Count(); i++)
            {
                result.Add(new ExchangeRate
                {
                    Currency = currency,
                    Date = exchangeRates.ElementAt(i).Date,
                    Rate = exponentialAverage1.ElementAt(i).Rate 
                        - exponentialAverage2.ElementAt(i).Rate
                });
            }
            return result;
        }
    }
}
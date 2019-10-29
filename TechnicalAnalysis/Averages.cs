using System.Collections.Generic;
using System.Linq;
using NBPApiClient.ExchangeRatesReader;

namespace NBPApiClient.TechnicalAnalysis
{
    internal static class Averages
    {
        internal static IEnumerable<ExchangeRate> MovingAverage(IEnumerable<ExchangeRate> exchangeRates, byte period)
        {
            if (!exchangeRates.Any())
                throw new EmptyListException("Nie można obliczyć średniej dla pustej kolekcji");

            var currency = exchangeRates.First().Currency;
            var movingAverage = new List<ExchangeRate>();

            for (int i = 1; i <= exchangeRates.Count(); i++)
            {
                var exchangeRate = new ExchangeRate
                {
                    Currency = currency,
                    Date = exchangeRates.ElementAt(i - 1).Date
                };
                if (i < period)
                {
                    decimal sum = 0;
                    for(int j = 1; j <= i; j++)
                        sum += exchangeRates.ElementAt(i - j).Rate;
                    exchangeRate.Rate = sum / i;
                }
                else
                {
                    decimal sum = 0;
                    for (int j = 1; j <= period; j++)
                        sum += exchangeRates.ElementAt(i - j).Rate;
                    exchangeRate.Rate = sum / period;
                }
                movingAverage.Add(exchangeRate);
            }
            return movingAverage;
        }
    }
}
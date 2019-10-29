using System.Collections.Generic;
using System.Linq;
using NBPApiClient.ExchangeRatesReader;
using System;

namespace NBPApiClient.TechnicalAnalysis
{
    internal static class Averages
    {
        /// <summary>
        /// Oblicza średnią kroczącą dla podanego kursu waluty
        /// </summary>
        /// <param name="exchangeRates"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        internal static IEnumerable<ExchangeRate> MovingAverage(IEnumerable<ExchangeRate> exchangeRates, int period)
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

        /// <summary>
        /// Oblicza średnią wykładniczą dla podanego kursu waluty
        /// </summary>
        /// <param name="exchangeRates"></param>
        /// <param name="period"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        internal static IEnumerable<ExchangeRate> ExponentialAverage(IEnumerable<ExchangeRate> exchangeRates, int period, double coefficient)
        {
            if (!exchangeRates.Any())
                throw new EmptyListException("Nie można obliczyć średniej dla pustej kolekcji");

            if(coefficient == 1)
                return MovingAverage(exchangeRates, period);

            if(coefficient == 0)
                throw new ArgumentException("Współczynnik nie może wynosić 0");

            var currency = exchangeRates.First().Currency;
            var exponentialAverage = new List<ExchangeRate>();
            
            for (int i = 1; i <= exchangeRates.Count(); i++)
            {
                var exchangeRate = new ExchangeRate
                {
                    Currency = currency,
                    Date = exchangeRates.ElementAt(i - 1).Date
                };
                if (i < period)
                {
                    double sum = 0;
                    double denominator = (1 - Math.Pow(coefficient, i))/(1 - coefficient);
                    for(int j = 1; j <= i; j++)
                        sum += (double)exchangeRates.ElementAt(i - j).Rate 
                            * Math.Pow(coefficient, j - 1);
                    exchangeRate.Rate = (decimal)(sum / denominator);
                }
                else
                {
                    double sum = 0;
                    double denominator = (1 - Math.Pow(coefficient, period))/(1 - coefficient);
                    for (int j = 1; j <= period; j++)
                        sum += (double)exchangeRates.ElementAt(i - j).Rate 
                            * Math.Pow(coefficient, j - 1);
                    exchangeRate.Rate = (decimal)(sum / denominator);
                }
                exponentialAverage.Add(exchangeRate);
            }
            return exponentialAverage;
        }
    }
}
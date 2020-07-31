using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NBPApiClient.ExchangeRatesReader;
using System.Linq;

namespace NBPApiClient
{
    internal static class FileOperations
    {
        /// <summary>
        /// Saving exchange rates of currency to csv file
        /// </summary>
        /// <param name="exchangeRates"></param>
        /// <param name="filePath"></param>
        internal static void SaveExchangeRateToCsv(string filePath, IEnumerable<ExchangeRate> exchangeRates)
        {
            var csv = new StringBuilder();
            foreach(ExchangeRate element in exchangeRates)
            {
                var date = element.Date.ToString("yyyy.MM.dd");
                var rate = element.Rate.ToString();
                var newLine = string.Format("{0}\t{1}", date, rate);
                csv.AppendLine(newLine);
            }
            File.WriteAllText(filePath, csv.ToString());
        }

        internal static void SaveSeriesToCsv(string filePath, IEnumerable<double> series)
        {
            var csv = new StringBuilder();
            foreach (double element in series)
                csv.AppendLine($"{element}");

            File.WriteAllText(filePath, csv.ToString());
        }

        internal static void SaveTwoSeriesToCsv(
            string filePath,
            double[] series1,
            double[] series2)
        {
            if (series1.Length != series2.Length)
                throw new ArgumentException("Serie danych musz¹ byæ równej d³ugoœci");

            var csv = new StringBuilder();
            for(var i = 0; i < series1.Count(); i++)
                csv.AppendLine($"{series1[i]}\t{series2[i]}");

            File.WriteAllText(filePath, csv.ToString());
        }
    }
}
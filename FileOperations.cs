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
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NBPApiClient.ExchangeRatesReader;
using System.Linq;

namespace NBPApiClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            //var exchangeRate = Utils.ReadExchangeRateForOneDay(Consts.DolarAmerykanski, new DateTime(2019, 10, 25));
            //var exchangeRatesForPeriod = 
            //    Utils.ReadExchangeRatesForPeriod(Consts.DolarAmerykanski, new DateTime(2019, 10, 1), new DateTime(2019, 10, 25));
            //foreach(ExchangeRate rate in exchangeRatesForPeriod.Result)
            //    Console.WriteLine(rate.Rate);
            if(args.Length == 0)
                ExecuteWithNoArguments();
            else if(args[0] == "save")
                ExecuteSavingTask(args[1]);
            else if(args[0] == "autocorrelation")
                await ExecuteAutocorrelation(args[1]);
            else
                ExecuteWithArguments(args);
            
        }
        private static void ExecuteWithNoArguments()
        {
            var usdRates = 
                Utils.ReadExchangeRatesForPeriod(Consts.DolarAmerykanski, 
                    DateTime.Today.AddYears(-1), DateTime.Today);
            var cadRates = 
                Utils.ReadExchangeRatesForPeriod(Consts.Euro, 
                    DateTime.Today.AddYears(-1), DateTime.Today);
            
            var corrCoefficient = StatisticOperations
                .GetCorrelationCoefficient(usdRates.Result.Select(x => x.Rate).ToArray(),
                    cadRates.Result.Select(x => x.Rate).ToArray());
            Console.WriteLine("Korelacja między " + 
                Consts.DolarAmerykanski + " a " + 
                Consts.Euro + " wynosi: " + corrCoefficient);
        }
        private static void ExecuteWithArguments(string[] args)
        {
            var r1Rates = 
                Utils.ReadExchangeRatesForPeriod(args[0], 
                    DateTime.Today.AddYears(-1), DateTime.Today);
            var r2Rates = 
                Utils.ReadExchangeRatesForPeriod(args[1], 
                    DateTime.Today.AddYears(-1), DateTime.Today);
            
            var corrCoefficient = StatisticOperations
                    .GetCorrelationCoefficient(r1Rates.Result.Select(x => x.Rate).ToArray(),
                        r2Rates.Result.Select(x => x.Rate).ToArray());
            Console.WriteLine("Korelacja między " + 
                args[0] + " a " + 
                args[1] + " wynosi: " + corrCoefficient);
        }
        private static void ExecuteSavingTask(string currency)
        {
            var rates = 
                Utils.ReadExchangeRatesForPeriod(currency, 
                    DateTime.Today.AddYears(-1), DateTime.Today);
            FileOperations
                .SaveExchangeRateToCsv(@"SavedFiles\" + currency + @".csv", rates.Result);
        }

        private static async Task ExecuteAutocorrelation(string currency)
        {
            IEnumerable<ExchangeRate> rates = await Utils.ReadExchangeRatesForPeriod(currency,
                DateTime.Today.AddYears(-1),
                DateTime.Today);


            double[] autocorrelation =
                StatisticOperations.GetAutoCorrelation(rates.Select(x => (double)x.Rate).ToArray());

            double[] normalizedSeries =
                StatisticOperations.GetNormalizedSeries(rates.Select(x => (double) x.Rate).ToArray());
            double[] autocorrelationOfNormalizedSeries = StatisticOperations.GetAutoCorrelation(normalizedSeries);
            if (!Directory.Exists("SavedFiles"))
                Directory.CreateDirectory("SavedFiles");

            //FileOperations
            //   .SaveSeriesToCsv(@"SavedFiles\" + currency + @"Autocorrelation.csv", autocorrelation);
            FileOperations.SaveTwoSeriesToCsv(@"SavedFiles\" + currency + @"Autocorrelation.csv", autocorrelation, autocorrelationOfNormalizedSeries);
        }
    }
}
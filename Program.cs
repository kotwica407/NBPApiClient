using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NBPApiClient.ExchangeRatesReader;
using NBPApiClient.TechnicalAnalysis;
using System.Linq;

namespace NBPApiClient
{
    class Program
    {
        static void Main(string[] args)
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
            else if(args[0] == "average")
                ExecuteCalculateMovingAverageTask(args[1]);
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
                .CorrelationCoefficient(usdRates.Result.Select(x => x.Rate).ToList(),
                    cadRates.Result.Select(x => x.Rate).ToList());
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
                    .CorrelationCoefficient(r1Rates.Result.Select(x => x.Rate).ToList(),
                        r2Rates.Result.Select(x => x.Rate).ToList());
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

        private static void ExecuteCalculateMovingAverageTask(string currency)
        {
            var rates = 
                Utils.ReadExchangeRatesForPeriod(currency, 
                    DateTime.Today.AddYears(-1), DateTime.Today);
            FileOperations
                .SaveExchangeRateToCsv(
                    @"SavedFiles\" + currency + @"MovingAverage.csv", 
                    Averages.MovingAverage(rates.Result, 20));
        }
    }
}
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
            if(args.Length == 0)
                ExecuteWithNoArguments();
            else if(args[0] == "save")
                ExecuteSavingTask(args[1]);
            else if(args[0] == "average1")
                ExecuteCalculateMovingAverageTask(args[1]);
            else if(args[0] == "average2")
                ExecuteCalculateExponentialAverageTask(args[1]);
            else if(args[0] == "MACD")
                ExecuteCalculateMACDTask(args[1]);
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
        private static void ExecuteCalculateExponentialAverageTask(string currency)
        {
            var rates = 
                Utils.ReadExchangeRatesForPeriod(currency, 
                    DateTime.Today.AddYears(-1), DateTime.Today);
            FileOperations
                .SaveExchangeRateToCsv(
                    @"SavedFiles\" + currency + @"ExponentialAverage.csv", 
                    Averages.ExponentialAverage(rates.Result, 20, 0.9));
        }
        private static void ExecuteCalculateMACDTask(string currency)
        {
            var rates = 
                Utils.ReadExchangeRatesForPeriod(currency, 
                    DateTime.Today.AddYears(-1), DateTime.Today);
            FileOperations
                .SaveExchangeRateToCsv(
                    @"SavedFiles\" + currency + @"MACD.csv", 
                    MACD.CalculateMACD(rates.Result));
        }
    }
}
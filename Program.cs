using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NBPApiClient.ExchangeRatesReader;
using NBPApiClient.StatisticOperations;
using System.Linq;

namespace NBPApiClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            //var exchangeRate = Utils.ReadExchangeRateForOneDay(Consts.DolarAmerykanski, new DateTime(2019, 10, 25));
            //var exchangeRatesForPeriod = 
            //    Utils.ReadExchangeRatesForPeriod(Consts.DolarAmerykanski, new DateTime(2019, 10, 1), new DateTime(2019, 10, 25));
            //foreach(ExchangeRate rate in exchangeRatesForPeriod.Result)
            //    Console.WriteLine(rate.Rate);
            if(args.Length == 0)
                ExecuteWithNoArguments();
            else
                ExecuteWithArguments(args);
            
        }
        private static void ExecuteWithNoArguments()
        {
            var usdRates = 
                Utils.ReadExchangeRatesForPeriod(Consts.DolarAmerykanski, 
                    new DateTime(2018, 10, 25), new DateTime(2019, 10, 25));
            var cadRates = 
                Utils.ReadExchangeRatesForPeriod(Consts.Euro, 
                    new DateTime(2018, 10, 25), new DateTime(2019, 10, 25));
            
            var corrCoefficient = StatisticOperations
                .StatisticOperations
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
                    new DateTime(2018, 10, 25), new DateTime(2019, 10, 25));
            var r2Rates = 
                Utils.ReadExchangeRatesForPeriod(args[1], 
                    new DateTime(2018, 10, 25), new DateTime(2019, 10, 25));
            
            var corrCoefficient = StatisticOperations
                .StatisticOperations
                    .CorrelationCoefficient(r1Rates.Result.Select(x => x.Rate).ToList(),
                        r2Rates.Result.Select(x => x.Rate).ToList());
            Console.WriteLine("Korelacja między " + 
                args[0] + " a " + 
                args[1] + " wynosi: " + corrCoefficient);
        }
    }
}
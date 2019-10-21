using System;

namespace NBPApiClient.ExchangeRatesReader
{
    public class ExchangeRate
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
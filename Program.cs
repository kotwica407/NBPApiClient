using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NBPApiClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            //TODO Chcę zrobić analizę korelacji kursów różnych walut
            ProcessRepositories().Wait();
        }
        private static async Task ProcessRepositories()
        {

            var stringTask = client.GetStringAsync("http://api.nbp.pl/api/exchangerates/tables/a/");

            var msg = await stringTask;
            Console.Write(msg);
        }
    }
}
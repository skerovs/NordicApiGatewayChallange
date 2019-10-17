using System;
using System.Net.Http;
using System.Threading.Tasks;
using NordicApiGateway;

namespace HttpClientSample
{

    class Program
    {
        static HttpClient client = new HttpClient();
        private readonly MonarchService monarchService;

        static async Task Main()
        {

            ApiHelper.InitializeClient();
            bool monarchsLoaded = await MonarchService.GetMonarchs();
            if (monarchsLoaded)
            {
                var numberOfMonarchs = MonarchService.GetMonarchsCount();
                var longestRulingMonarch = MonarchService.GetLongestRulingMonarch();
                var longestRulingHousehold = MonarchService.GetLongestRulingHousehold();
                var mostCommonName = MonarchService.GetMostCommonName();

                Console.WriteLine($"The number of monarchs is { numberOfMonarchs }.");
                Console.WriteLine($"The longest-ruling monarch is { longestRulingMonarch.Name }, who ruled for { longestRulingMonarch.NumberOfRulingYears } years.");
                Console.WriteLine($"The longest-ruling household is { longestRulingHousehold.Name }, they ruled for {longestRulingHousehold.RulingYears} years.");
                Console.WriteLine($"The most common name between monarchs is { mostCommonName }.");
            }
        }
    }
}
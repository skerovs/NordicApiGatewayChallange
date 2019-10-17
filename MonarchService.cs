using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NordicApiGateway
{
    public class MonarchService
    {
        private static List<MonarchModel> MonarchsRepo { get; set; }

        public static async Task<bool> GetMonarchs()
        {
            string url =
                "christianpanton/10d65ccef9f29de3acd49d97ed423736/raw/b09563bc0c4b318132c7a738e679d4f984ef0048/kings";

            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string monarchsString = await response.Content.ReadAsStringAsync();
                MonarchsRepo = !string.IsNullOrEmpty(monarchsString) ? JsonConvert.DeserializeObject<List<MonarchModel>>(monarchsString) : new List<MonarchModel>();
                ProcessMonarchsYears();
                return true;
            }

            throw new Exception(response.ReasonPhrase);
        }

        public static int GetMonarchsCount()
        {
            return MonarchsRepo.Count;
        }

        public static MonarchModel GetLongestRulingMonarch()
        {
            return MonarchsRepo.OrderByDescending(x => x.NumberOfRulingYears).First();
        }

        public static HouseholdModel GetLongestRulingHousehold()
        {
            return MonarchsRepo.GroupBy(h => h.Household).Select(ho => new HouseholdModel
            {
                Name = ho.First().Household,
                RulingYears = ho.Sum(x => x.NumberOfRulingYears)
            }).OrderByDescending(x => x.RulingYears).First();
        }

        public static string GetMostCommonName()
        {
            return MonarchsRepo.SelectMany(x => x.Name.Split(" "))
                .ToArray().GroupBy(n => n).Select(c => new {c.Key, Count = c.Count()}).OrderByDescending(x => x.Count).First().Key;
        }

        private static void ProcessMonarchsYears()
        {
            foreach (var monarchModel in MonarchsRepo)
            {
                var years = monarchModel.RulingYears.Split('-').ToList();
                int i = 0;
                foreach (var year in years)
                {
                    if (int.TryParse(year, out var parsedYear))
                    {
                        if (i == 0)
                            monarchModel.RulingStartYear = parsedYear;
                        if(i > 0 || years.Count == 1)
                            monarchModel.RulingEndYear = parsedYear;
                    }
                    else
                        Console.WriteLine($"Problem with parsing monarch ruling years with ID: {monarchModel.ID}");
                    i++;
                }
                monarchModel.NumberOfRulingYears = monarchModel.RulingEndYear - monarchModel.RulingStartYear == 0 ? 1 : monarchModel.RulingEndYear - monarchModel.RulingStartYear;
            } 
        }
    }
}

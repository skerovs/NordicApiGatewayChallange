
using Newtonsoft.Json;

namespace NordicApiGateway
{
    public class MonarchModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("nm")]
        public string Name { get; set; }

        [JsonProperty("hse")]
        public string Household { get; set; }

        [JsonProperty("cty")]
        public string City { get; set; }

        [JsonProperty("yrs")]
        public string RulingYears { get; set; }

        public int RulingStartYear { get; set; }
        public int RulingEndYear { get; set; }
        public int NumberOfRulingYears { get; set; }
    }
}

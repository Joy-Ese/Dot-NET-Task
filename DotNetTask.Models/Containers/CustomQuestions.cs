using Newtonsoft.Json;

namespace DotNetTask.Models.Containers
{
    public class CustomQuestions
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "quest")]
        public string Question { get; set; } = string.Empty;
    }
}

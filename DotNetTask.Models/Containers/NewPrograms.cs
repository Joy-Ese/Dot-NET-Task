using Newtonsoft.Json;

namespace DotNetTask.Models.Containers
{
    public class NewPrograms
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "prog_Title")]
        public string Program_Title { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "prog_Desc")]
        public string Program_Description { get; set; } = string.Empty;
    }
}

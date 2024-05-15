using Newtonsoft.Json;

namespace DotNetTask.Models.Containers
{
    public class CandidateQuestions
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "candidate_UserId")]
        public string Candidate_UserId { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; } = string.Empty;
    }
}

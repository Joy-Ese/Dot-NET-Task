using Newtonsoft.Json;

namespace DotNetTask.Models.Containers
{
    public class Candidates
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "firstName")]
        public string First_Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "lastName")]
        public string Last_Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "nationality")]
        public string Nationality { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "currentResidence")]
        public string Current_Residence { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "idNumber")]
        public int Id_Number { get; set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public string Date_Of_Birth { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; } = string.Empty;
    }
}

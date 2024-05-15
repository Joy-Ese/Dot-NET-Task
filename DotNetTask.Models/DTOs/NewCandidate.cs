using System.ComponentModel.DataAnnotations;

namespace DotNetTask.Models.DTOs
{
    public class NewCandidate
    {
        [Required(ErrorMessage = "First Name is mandatory")]
        public string firstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is mandatory")]
        public string lastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is mandatory")]
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string nationality { get; set; } = string.Empty;
        public string currentResidence { get; set; } = string.Empty;
        public int idNumber { get; set; }
        public string dateOfBirth { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public List<AdditionalQuestions> additionals { get; set; }
    }

    public class AdditionalQuestions
    {
        public string question { get; set; } = string.Empty;
        public string answer { get; set; } = string.Empty;
    }
}

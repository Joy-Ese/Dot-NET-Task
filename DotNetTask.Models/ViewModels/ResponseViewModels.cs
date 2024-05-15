using DotNetTask.Models.DTOs;

namespace DotNetTask.Models.ViewModels
{
    public class ResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; } = string.Empty;
    }

    public class GetEndpointsResponseModel<T>
    {
        public bool status { get; set; }
        public string message { get; set; } = string.Empty;
        public T result { get; set; }
    }

    public class GetCandidatesViewModel
    {
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string nationality { get; set; } = string.Empty;
        public string currentResidence { get; set; } = string.Empty;
        public int idNumber { get; set; }
        public string dateOfBirth { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public List<AdditionalQuestions> additionals { get; set; }
    }
}

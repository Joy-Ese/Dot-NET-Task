using DotNetTask.Models.DTOs;
using DotNetTask.Models.ViewModels;

namespace DotNetTask.Services.Interfaces
{
    public interface ICandidateService
    {
        Task<ResponseModel> AddCandidate(NewCandidate req);
        Task<ResponseModel> AddCustomQuestion(NewQuestion req);
        Task<ResponseModel> AddNewProgram(NewProgram req);
        Task<GetEndpointsResponseModel<List<GetCandidatesViewModel>>> GetAllCandidates();
        Task<GetEndpointsResponseModel<List<GetQuestionByTypeViewModel>>> GetQuestionByType(string type);
        Task<ResponseModel> UpdateCandidateQuestion(string id, string quest);
    }
}

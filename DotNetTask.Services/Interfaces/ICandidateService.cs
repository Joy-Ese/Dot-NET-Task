using DotNetTask.Models.DTOs;
using DotNetTask.Models.ViewModels;

namespace DotNetTask.Services.Interfaces
{
    public interface ICandidateService
    {
        Task<ResponseModel> AddCandidate(NewCandidate req);
        Task<GetEndpointsResponseModel<List<GetCandidatesViewModel>>> GetAllCandidates();
    }
}

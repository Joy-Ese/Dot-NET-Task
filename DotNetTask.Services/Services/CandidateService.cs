using DotNetTask.Models.Containers;
using DotNetTask.Models.DTOs;
using DotNetTask.Models.ViewModels;
using DotNetTask.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace DotNetTask.Services.Services
{
    public  class CandidateService : ICandidateService
    {
        private readonly ILogger<CandidateService> _logger;
        private readonly Container _container;
        private readonly Container _container2;

        public CandidateService(ILogger<CandidateService> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _logger.LogDebug(1, "Nlog injected into the CandidateService");
            _container = cosmosClient.GetContainer("DotNetTaskDB", "Candidates");
            _container2 = cosmosClient.GetContainer("DotNetTaskDB", "CandidateQuestions");
        }

        public async Task<ResponseModel> AddCandidate(NewCandidate req)
        {
            ResponseModel respModel = new ResponseModel();
            try
            {
                Candidates newCandidate = new Candidates
                {
                    First_Name = req.firstName,
                    Last_Name = req.lastName,
                    Email = req.email,
                    Phone = req.phone,
                    Nationality = req.nationality,
                    Current_Residence = req.currentResidence,
                    Id_Number = req.idNumber,
                    Date_Of_Birth = req.dateOfBirth,
                    Gender = req.gender
                };
                ItemResponse<Candidates> response = await _container.CreateItemAsync(newCandidate);
                var savedCandidate = response.Resource;

                _logger.LogInformation($"Successfully created Candidate ----------- {savedCandidate} ---------------------");

                foreach (var item in req.additionals)
                {
                    CandidateQuestions newCandidateQuestion = new CandidateQuestions
                    {
                        Candidate_UserId = savedCandidate.Id,
                        Question = item.question,
                        Answer = item.answer
                    };
                    ItemResponse<CandidateQuestions> response2 = await _container2.CreateItemAsync(newCandidateQuestion);
                }


                respModel.status = true;
                respModel.message = $"Successfully added {savedCandidate.First_Name} as a candidate";
                return respModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
                return respModel;
            }
        }

        public async Task<GetEndpointsResponseModel<List<GetCandidatesViewModel>>> GetAllCandidates()
        {
            var candidateList = new List<GetCandidatesViewModel>();
            try
            {
                FeedIterator<Candidates> iterator1 = _container.GetItemQueryIterator<Candidates>();
                FeedIterator<CandidateQuestions> iterator2 = _container2.GetItemQueryIterator<CandidateQuestions>();

                while (iterator1.HasMoreResults)
                {
                    FeedResponse<Candidates> response1 = await iterator1.ReadNextAsync();
                    foreach (var item in response1)
                    {
                        var candidate = new GetCandidatesViewModel();

                        candidate.firstName = item.First_Name;
                        candidate.lastName = item.Last_Name;
                        candidate.email = item.Email;
                        candidate.phone = item.Phone;
                        candidate.nationality = item.Nationality;
                        candidate.currentResidence = item.Current_Residence;
                        candidate.idNumber = item.Id_Number;
                        candidate.dateOfBirth = item.Date_Of_Birth;
                        candidate.gender = item.Gender;

                        var additionalList = new List<AdditionalQuestions>();

                        iterator2 = _container2.GetItemQueryIterator<CandidateQuestions>();

                        while (iterator2.HasMoreResults)
                        {
                            FeedResponse<CandidateQuestions> response2 = await iterator2.ReadNextAsync();
                            foreach (var item2 in response2)
                            {
                                if (item.Id == item2.Candidate_UserId)
                                {
                                    var additional = new AdditionalQuestions();

                                    additional.question = item2.Question;
                                    additional.answer = item2.Answer;

                                    additionalList.Add(additional);
                                }
                            }
                        }

                        candidate.additionals = additionalList;
                        candidateList.Add(candidate);
                    };
                }

                return new GetEndpointsResponseModel<List<GetCandidatesViewModel>>
                {
                    status = true,
                    message = "Successful",
                    result = candidateList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
                return new GetEndpointsResponseModel<List<GetCandidatesViewModel>>
                {
                    status = false,
                    message = "Failed",
                    result = candidateList
                };
            }
        }

    }
}

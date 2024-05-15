using Azure;
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
        private readonly Container _container3;
        private readonly Container _container4;

        public CandidateService(ILogger<CandidateService> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _logger.LogDebug(1, "Nlog injected into the CandidateService");
            _container = cosmosClient.GetContainer("DotNetTaskDB", "Candidates");
            _container2 = cosmosClient.GetContainer("DotNetTaskDB", "CandidateQuestions");
            _container3 = cosmosClient.GetContainer("DotNetTaskDB", "CustomQuestions");
            _container4 = cosmosClient.GetContainer("DotNetTaskDB", "NewPrograms");
        }

        public async Task<ResponseModel> AddCandidate(NewCandidate req)
        {
            ResponseModel respModel = new ResponseModel();
            try
            {
                // dynamically add the id
                int lastId = 0;
                int newId = 0;

                var lastCandidate = _container.GetItemLinqQueryable<Candidates>().OrderByDescending(d => d.Id).FirstOrDefault();
                if (lastCandidate != null)
                {
                    lastId = int.Parse(lastCandidate.Id);
                    newId = lastId + 1;
                }
                else
                {
                    newId = 1;
                }


                Candidates newCandidate = new Candidates
                {
                    Id = newId.ToString(),
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

                _logger.LogInformation($"Successfully created a new Candidate ----------- {savedCandidate} ---------------------");

                foreach (var item in req.additionals)
                {
                    // dynamically add the id
                    var lastCandidateQuest = _container.GetItemLinqQueryable<CandidateQuestions>().OrderByDescending(d => d.Id).FirstOrDefault();
                    if (lastCandidateQuest != null)
                    {
                        lastId = int.Parse(lastCandidateQuest.Id);
                        newId = lastId + 1;
                    }
                    else
                    {
                        newId = 1;
                    }


                    CandidateQuestions newCandidateQuestion = new CandidateQuestions
                    {
                        Id = newId.ToString(),
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
                respModel.status = false;
                respModel.message = "Failed";
                return respModel;
            }
        }

        public async Task<ResponseModel> AddCustomQuestion(NewQuestion req)
        {
            ResponseModel respModel = new ResponseModel();
            try
            {
                int lastId = 0;
                int newId = 0;

                var lastCustQues = _container3.GetItemLinqQueryable<CustomQuestions>().OrderByDescending(d => d.Id).FirstOrDefault();
                if (lastCustQues != null)
                {
                    lastId = int.Parse(lastCustQues.Id);
                    newId = lastId + 1;
                }
                else
                {
                    newId = 1;
                }

                CustomQuestions newCustQuest = new CustomQuestions
                {
                    Id = newId.ToString(),
                    Type = req.type,
                    Question = req.quest
                };
                ItemResponse<CustomQuestions> response = await _container3.CreateItemAsync(newCustQuest);

                _logger.LogInformation($"Successfully created a new Custom Question ----------- {response.Resource} ---------------------");

                respModel.status = true;
                respModel.message = $"Successful";
                return respModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
                respModel.status = false;
                respModel.message = "Failed";
                return respModel;
            }
        }

        public async Task<ResponseModel> AddNewProgram(NewProgram req)
        {
            ResponseModel respModel = new ResponseModel();
            try
            {
                int lastId = 0;
                int newId = 0;

                var lastProg = _container4.GetItemLinqQueryable<NewPrograms>().OrderByDescending(d => d.Id).FirstOrDefault();
                if (lastProg != null)
                {
                    lastId = int.Parse(lastProg.Id);
                    newId = lastId + 1;
                }
                else
                {
                    newId = 1;
                }

                NewPrograms newProg = new NewPrograms
                {
                    Id = newId.ToString(),
                    Program_Title = req.prog_Title,
                    Program_Description = req.prog_Desc
                };
                ItemResponse<NewPrograms> response = await _container4.CreateItemAsync(newProg);

                _logger.LogInformation($"Successfully created a new Program ----------- {response.Resource} ---------------------");

                respModel.status = true;
                respModel.message = $"Successful";
                return respModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
                respModel.status = false;
                respModel.message = "Failed";
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

                    _logger.LogInformation($"Successfully fetched Candidates from the DB ----------- {response1.Resource} -----------------");

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

                            _logger.LogInformation($"Successfully fetched CandidateQuestions from the DB ----- {response2.Resource} ----------");

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

        public async Task<GetEndpointsResponseModel<List<GetQuestionByTypeViewModel>>> GetQuestionByType(string type)
        {
            var listOfQuesByType = new List<GetQuestionByTypeViewModel>();
            try
            {
                FeedIterator<CustomQuestions> iterator = _container3.GetItemQueryIterator<CustomQuestions>();
                
                while (iterator.HasMoreResults)
                {
                    FeedResponse<CustomQuestions> questions = await iterator.ReadNextAsync();

                    _logger.LogInformation($"Successfully fetched Custom Questions from the DB --------- {questions.Resource} --------");

                    foreach (var item in questions)
                    {
                        if (item.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                        {
                            var getQuestion = new GetQuestionByTypeViewModel();
                            getQuestion.quest = item.Question;

                            listOfQuesByType.Add(getQuestion);
                        }
                    };
                }

                return new GetEndpointsResponseModel<List<GetQuestionByTypeViewModel>>
                {
                    status = true,
                    message = "Successful",
                    result = listOfQuesByType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
                return new GetEndpointsResponseModel<List<GetQuestionByTypeViewModel>>
                {
                    status = false,
                    message = "Failed",
                    result = listOfQuesByType
                };
            }
        }

        public async Task<ResponseModel> UpdateCandidateQuestion(string id, string quest) 
        {
            ResponseModel respModel = new ResponseModel();
            try
            {
                var query = _container2.GetItemLinqQueryable<CandidateQuestions>().Where(d => d.Candidate_UserId == id)
                    .OrderByDescending(d => d.Id).Take(1);

                var lastAddedQuestion = query.FirstOrDefault();

                if (lastAddedQuestion == null)
                {
                    respModel.status = false;
                    respModel.message = "User's Record does not exist!";
                    return respModel;
                }

                lastAddedQuestion.Question = quest;

                var response = await _container2.ReplaceItemAsync(lastAddedQuestion, lastAddedQuestion.Id);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    respModel.status = true;
                    respModel.message = "User's Question updated successfully!";
                }
                else
                {
                    respModel.status = false;
                    respModel.message = "Failed to update User's Question!";
                }

                return respModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
                respModel.status = false;
                respModel.message = "Failed";
                return respModel;
            }
        }

    }
}

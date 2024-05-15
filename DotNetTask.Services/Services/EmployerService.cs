using DotNetTask.Models.Containers;
using DotNetTask.Models.DTOs;
using DotNetTask.Models.ViewModels;
using DotNetTask.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;

namespace DotNetTask.Services.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly ILogger<EmployerService> _logger;
        private readonly Container _container;
        private readonly Container _container2;

        public EmployerService(ILogger<EmployerService> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _logger.LogDebug(1, "Nlog injected into the CandidateService");
            _container = cosmosClient.GetContainer("DotNetTaskDB", "NewPrograms");
            _container2 = cosmosClient.GetContainer("DotNetTaskDB", "CustomQuestions");
        }

        public async Task<ResponseModel> AddCustomQuestion(NewQuestion req)
        {
            ResponseModel respModel = new ResponseModel();
            try
            {
                int lastId = 0;
                int newId = 0;

                var lastCustQues = _container.GetItemLinqQueryable<CustomQuestions>().OrderByDescending(d => d.Id).FirstOrDefault();
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
                ItemResponse<CustomQuestions> response = await _container.CreateItemAsync(newCustQuest);

                _logger.LogInformation($"Successfully created a new Custom Question ----------- {response.Resource} ---------------------");

                respModel.status = true;
                respModel.message = $"Successful";
                return respModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
                return respModel;
            }
        }

    }
}

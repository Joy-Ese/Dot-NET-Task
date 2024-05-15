using DotNetTask.Models.DTOs;
using DotNetTask.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTask.Services.Interfaces
{
    public interface IEmployerService
    {
        Task<ResponseModel> AddCustomQuestion(NewQuestion req);
    }
}

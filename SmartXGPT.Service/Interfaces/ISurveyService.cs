using SmartXGPT.Service.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Interfaces
{
    public interface ISurveyService
    {
        Task<SurveyResultDto> AddSurvey(SurveyResultDto surveyDto);
        Task<SurveyResultDto> GetSurvey(Guid? surveyId);
        Task<List<SurveyResultDto>> GetAllServeys();
        Task<SurveyResultDto> UpdateSurvey(SurveyResultDto surveyDto);
    }
}

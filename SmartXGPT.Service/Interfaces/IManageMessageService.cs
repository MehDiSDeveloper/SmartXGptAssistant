using SmartXGPT.Service.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Interfaces
{
    public interface IManageMessageService
    {
        MesssageGPTDto CreateSimpleMessage(SurveyResultDto surveyDto);
        MesssageGPTDto CreateSmsMessage(SurveyResultDto surveyDto);
        MesssageGPTDto CreateCommentSmsMessage(SurveyResultDto surveyDto);        
        Task<MesssageGPTDto> AskSimpleMessage(MesssageGPTDto messageDto);
        Task<MesssageGPTDto> AskTranslatedMessage(MesssageGPTDto messageDto);
        Task<MesssageGPTDto> AddMessage(MesssageGPTDto messageDto);
        Task<MesssageGPTDto> CreateHierarchyMessage(SurveyResultDto surveyDto,MesssageGPTDto messageDto);
        Task<MesssageGPTDto> AskManualQuestion(MesssageGPTDto messageDto);
        Task<MesssageGPTDto> AskManualQuestionWithContext(MesssageGPTDto messageDto);
        Task<bool> RemoveMessage(Guid messageId);
        Task<bool> RemoveQuestion(Guid QuestionId);
        Task<bool> DeleteMessage(Guid messageId);
        Task<MesssageGPTDto> GetMessage(Guid? messageId);
        

    }
}

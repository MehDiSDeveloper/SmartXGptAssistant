using AutoMapper;
using SmartXGPT.Infrastructure.Contexts.SmartXDbContext;
using SmartXGPT.Service.Interfaces;
using SmartXGPT.Service.Models.DTOs;
using SmartXGPT.Service.Models.Entities;
using SmartXGPT.Infrastructure.AutoMapperProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartXGPT.Service.Models.Entities;
namespace SmartXGPT.Infrastructure.APIServices
{
    public class SurveyService : ISurveyService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly IManageMessageService _manageMessageService;

        public SurveyService(IMapper mapper, ApplicationDbContext db, IManageMessageService manageMessageService)
        {
            _mapper = mapper;
            _db = db;
            _manageMessageService = manageMessageService;
        }



        public async Task<SurveyResultDto> AddSurvey(SurveyResultDto surveyDto)
        {
            try
            {
                var survey = _mapper.Map<SurveyResult>(surveyDto);

                if (surveyDto.Id == null)
                {
                    await _db.SurveyResults.AddAsync(survey);
                }
                else
                {
                    _db.SurveyResults.Update(survey);
                }
                surveyDto = _mapper.Map<SurveyResultDto>(survey);


                if (surveyDto.DoSimple)
                {
                    MesssageGPTDto simpleMessage = _manageMessageService.CreateSimpleMessage(surveyDto);

                       simpleMessage.Type = MessageType.Simple;
                    simpleMessage = await _manageMessageService.AskSimpleMessage(simpleMessage);
                    simpleMessage.Survey = survey;
                    simpleMessage.SurveyId = survey.Id;
                    simpleMessage = await _manageMessageService.AddMessage(simpleMessage);
                }

                if (surveyDto.DoTranslate)
                {
                    MesssageGPTDto translatedMessage = _manageMessageService.CreateSimpleMessage(surveyDto);

                    translatedMessage.Type = MessageType.Translated;
                    translatedMessage = await _manageMessageService.AskTranslatedMessage(translatedMessage);
                    translatedMessage.Survey = survey;
                    translatedMessage.SurveyId = survey.Id;
                    translatedMessage.Id = null;
                    translatedMessage = await _manageMessageService.AddMessage(translatedMessage);
                }

                if (surveyDto.DoSms)
                {
                    MesssageGPTDto smsMessage = _manageMessageService.CreateSmsMessage(surveyDto);
                    smsMessage.Type = MessageType.sms;
                    smsMessage = await _manageMessageService.AskTranslatedMessage(smsMessage);
                    smsMessage.Survey = survey;
                    smsMessage.SurveyId = survey.Id;
                    smsMessage.Id = null;
                    smsMessage = await _manageMessageService.AddMessage(smsMessage);
                }

                if (surveyDto.DoCommentSms && !string.IsNullOrEmpty(surveyDto.Comment))
                {
                    MesssageGPTDto commentSmsMessage = _manageMessageService.CreateCommentSmsMessage(surveyDto);
                    commentSmsMessage.Type = MessageType.sms;
                    commentSmsMessage = await _manageMessageService.AskTranslatedMessage(commentSmsMessage);
                    commentSmsMessage.Survey = survey;
                    commentSmsMessage.SurveyId = survey.Id;
                    commentSmsMessage.Id = null;
                    commentSmsMessage = await _manageMessageService.AddMessage(commentSmsMessage);
                }

                if (surveyDto.DoHierarchy)
                {
                    MesssageGPTDto hierarchyMessage = new();
                    hierarchyMessage.Type = MessageType.TranslatedHierarchy;
                    hierarchyMessage.UsedTokens = 0;
                    hierarchyMessage.Hierarchy = new Dictionary<string, string>(2);
                    foreach (MessageStep step in Enum.GetValues(typeof(MessageStep)))
                    {
                        hierarchyMessage.Step = step;
                        hierarchyMessage = await _manageMessageService.CreateHierarchyMessage(surveyDto, hierarchyMessage);
                        string key = hierarchyMessage.Question;
                        hierarchyMessage = await _manageMessageService.AskTranslatedMessage(hierarchyMessage);
                        string value = hierarchyMessage.FaAnswer;
                        hierarchyMessage.Hierarchy.Add(key, value);
                        hierarchyMessage.UsedTokens += hierarchyMessage.FaQuestion.Length + hierarchyMessage.FaAnswer.Length;
                    }
                    hierarchyMessage.Survey = survey;
                    hierarchyMessage.SurveyId = survey.Id;
                    hierarchyMessage = await _manageMessageService.AddMessage(hierarchyMessage);
                }

                surveyDto = _mapper.Map<SurveyResultDto>(survey);
                surveyDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                surveyDto.ErrorMessage = ex.Message;
                surveyDto.IsSuccess = false;
            }
            return surveyDto;
        }

        public async Task<List<SurveyResultDto>> GetAllServeys()
        {
            //return _mapper.Map<List<SurveyResultDto>>(await _db.SurveyResults.Where(s => !s.IsRemoved).OrderByDescending(s => s.InsertTime).ToListAsync());//
            return  _mapper.Map<List<SurveyResultDto>>(await _db.SurveyResults.FromSqlRaw("GetAllSurveys").ToListAsync());
            
        }

        public async Task<SurveyResultDto> GetSurvey(Guid? surveyId)
        {

            if (surveyId == null)
            {
                SurveyResultDto surveyDto = new()
                {
                    IsSuccess = false
                };

                return surveyDto;
            }
            else
            {
                var survey = await _db.SurveyResults.Include(s => s.Messages.Where(m => !m.IsRemoved).OrderByDescending(m => m.InsertTime)).ThenInclude(m => m.Questions.Where(q => !q.IsRemoved).OrderBy(q => q.InsertTime)).ThenInclude(q => q.Answer).SingleOrDefaultAsync(s => s.Id == surveyId);
                return _mapper.Map<SurveyResultDto>(survey);
            }
        }

        public async Task<SurveyResultDto> UpdateSurvey(SurveyResultDto surveyDto)
        {
            var survey = await _db.SurveyResults.FirstOrDefaultAsync(s => s.Id == surveyDto.Id);
            survey = _mapper.Map<SurveyResult>(surveyDto);
            _db.SurveyResults.Update(survey);
            await _db.SaveChangesAsync();
            return surveyDto;
        }
    }
}

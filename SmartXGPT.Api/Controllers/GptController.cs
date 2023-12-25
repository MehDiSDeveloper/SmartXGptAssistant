using Azure;
using Microsoft.AspNetCore.Mvc;
using SmartXGPT.Api.Models;
using SmartXGPT.Service.Interfaces;
using SmartXGPT.Service.Models.DTOs;
using SmartXGPT.Service.Models.Entities;

namespace SmartXGPT.Web.Controllers
{
    public class GptController : Controller
    {
        private readonly IManageMessageService _manageMessageService;
        private readonly ISurveyService _surveyService;
        private readonly IGPTAPIService _gpTAPIService;
        public GptController(IManageMessageService manageMessageService, ISurveyService surveyService, IGPTAPIService gPTAPIService)
        {
            _manageMessageService = manageMessageService;
            _surveyService = surveyService;
            _gpTAPIService = gPTAPIService;
        }

        [HttpGet]
        public async Task<IActionResult> Assistant()
        {
            
            return View(new AssistantReqDto());
        }
        [HttpPost]
        public async Task<IActionResult> Assistant(AssistantReqDto assistantReqDto)
        {
            assistantReqDto = await _gpTAPIService.AskGPTAssistantAsync(assistantReqDto);
            return View(assistantReqDto);
        }


        [HttpGet]
        public async Task<IActionResult> SurveyListIndex()
        {
            var surveys = await _surveyService.GetAllServeys();
            return View(surveys);
        }







        [HttpGet]
        public IActionResult GptManualCall()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GptManualCall(MesssageGPTDto messageDto)
        {
            MesssageGPTDto response = await _manageMessageService.AskManualQuestion(messageDto);
            return View(response);
        }



        [HttpGet]
        public IActionResult GptManualCallWithContext()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GptManualCallWithContext(MesssageGPTDto messageDto)
        {
            MesssageGPTDto response = await _manageMessageService.AskManualQuestionWithContext(messageDto);
            return RedirectToAction("ViewMessage", new { id = response.Id });
        }


        [HttpGet]
        public async Task<IActionResult> ViewMessage(Guid id)
        {
            var message = await _manageMessageService.GetMessage(id);
            if (message != null)
                message.FaQuestion = "";
            ViewBag.Res = ViewBag.IsDeleted;
            return View(message);
        }




        [HttpGet]
        public IActionResult NewSurvey()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewSurvey(SurveyResultDto surveyDto)
        {
            if (ModelState.IsValid)
            {
                surveyDto = await _surveyService.AddSurvey(surveyDto);
                return RedirectToAction(nameof(ViewSurvey), surveyDto);
            }
            else
            {
                ViewBag.IsSuccess = false;
                return View(surveyDto);
            }
        }



        [HttpGet]
        public async Task<IActionResult> ViewSurvey(Guid? id)
        {
            var survey = await _surveyService.GetSurvey(id);
            return View(survey);
        }

        [HttpGet]
        public async Task<IActionResult> EditSurvey(Guid id)
        {
            var survey = await _surveyService.GetSurvey(id);
            return View(survey);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMessage(Guid messageId, Guid surveyId)
        {
            bool res = await _manageMessageService.DeleteMessage(messageId);
            ViewBag.IsDeleted = res;
            return RedirectToAction("ViewSurvey", new { id = surveyId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteQuestion(Guid questionId, Guid messageId)
        {

            var done = await _manageMessageService.RemoveQuestion(questionId);
            ViewBag.IsDeleted = done;
            return RedirectToAction("ViewMessage", new { id = messageId });
        }
    }
}

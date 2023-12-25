using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SmartXGPT.Infrastructure.Contexts.SmartXDbContext;
using SmartXGPT.Service.Interfaces;
using SmartXGPT.Service.Models.DTOs;
using SmartXGPT.Service.Models.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Infrastructure.APIServices
{
    public class ManageMessageService : IManageMessageService
    {
        private readonly IGPTAPIService _gPTAPIService;
        private readonly ITranslationService _translationService;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ManageMessageService(IGPTAPIService gPTAPIservice, ITranslationService translationService, ApplicationDbContext db, IMapper mapper)
        {
            _gPTAPIService = gPTAPIservice;

            _translationService = translationService;
            _db = db;
            _mapper = mapper;
        }



        public string ReOrderFarsiText(string st)
        {
            string text = "<p class=\"text-muted\">";
            st = st.Replace("\n", "<br>");
            text += st + "</p>";
            return text;
        }

        public async Task<MesssageGPTDto> AskSimpleMessage(MesssageGPTDto messageDto)
        {
            messageDto.AskThis = messageDto.FaQuestion;
            messageDto = await _gPTAPIService.AskGPTAsync(messageDto);
            messageDto.FaAnswer = messageDto.RawAnswer;
            messageDto.FaAnswer = ReOrderFarsiText(messageDto.FaAnswer);
            return messageDto;
        }

        public async Task<MesssageGPTDto> AskTranslatedMessage(MesssageGPTDto messageDto)
        {
            messageDto.IsAsked = false;
            messageDto = await _translationService.TrMessage(messageDto);
            messageDto.AskThis = messageDto.EnQuestion;
            messageDto = await _gPTAPIService.AskGPTAsync(messageDto);
            messageDto.EnAnswer = messageDto.RawAnswer;
            messageDto = await _translationService.TrMessage(messageDto);
            messageDto.RawAnswer = messageDto.FaAnswer;
            messageDto.FaAnswer = ReOrderFarsiText(messageDto.FaAnswer);
            return messageDto;
        }

        public MesssageGPTDto CreateSimpleMessage(SurveyResultDto surveyDto)
        {
            #region add scores to query
            Dictionary<string, Score> scores = new Dictionary<string, Score>(5);
            scores.Add("کیفیت غذا ", surveyDto.FoodQuality);
            scores.Add("سطح بهداشت ", surveyDto.Cleanliness);
            scores.Add("قیمت ", surveyDto.Price);
            scores.Add("رفتار پرسنل ", surveyDto.Behavior);


            var worstscores = scores.Where(s => s.Value == Score.افتضاح || s.Value == Score.بد || s.Value == Score.معمولی);
            var bestscores = scores.Where(s => s.Value == Score.عالی || s.Value == Score.خوب);

            bestscores = bestscores.OrderByDescending(s => s.Value);
            var bests = bestscores.Take(2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            worstscores = worstscores.OrderBy(s => s.Value);
            var worsts = worstscores.Take(2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


            string query1 = "نظر او";
            string query2 = " نظر او";

            if (worsts != null)
            {
                foreach (var item in worsts)
                {
                    query1 += " در مورد " + item.Key + item.Value + " و ";
                }
            }
            if (bests != null)
            {
                foreach (var item in bests)
                {
                    query2 += " در مورد " + item.Key + item.Value + " و ";
                }
                if (query2.EndsWith(" و "))
                {
                    query2 = query2.Substring(0, query2.Length - 3);
                    query2 += " است.";
                }
                if (worsts != null)
                {
                    query1 = query1.Substring(0, query1.Length - 3);
                    query1 += " و همچنین ";
                }
                else query1 = "";
            }
            else
            {
                if (worsts == null)
                {
                    query1 = $"نظر او در مورد رستوران {surveyDto.Overall} است";
                    query2 = "";
                }
                query1 = query1.Substring(0, query1.Length - 3);
                query1 += " است.";
                query2 = "";
            }
            #endregion

            string Query = $"{surveyDto.CustomerName} مشتری رستوران {surveyDto.RestaurantName} فرم نظر سنجی رستوران را پر کرده است.\n";
            Query += query1 + query2;
            Query += $"\n به صورت لیست شماره دار در خطوط مجزا با در نظر گرفتن نظرات {surveyDto.CustomerName} " +
                $"بگو قبل از تماس تلفنی با او باید چکار کنم و " +
                $"در حین تماس تلفنی چه چیزهایی بگویم";

            MesssageGPTDto messsageDto = new()
            {
                FaQuestion = Query,
                SurveyId = surveyDto.Id,
                LimitTokens = surveyDto.LimitTokens - Query.Length,
                Temperature = surveyDto.Temperature,
            };
            return messsageDto;
        }

        public MesssageGPTDto CreateSmsMessage(SurveyResultDto surveyDto)
        {
            #region add scores to query
            Dictionary<string, Score> scores = new Dictionary<string, Score>(5);
            scores.Add("کیفیت غذا ", surveyDto.FoodQuality);
            scores.Add("سطح بهداشت ", surveyDto.Cleanliness);
            scores.Add("قیمت ", surveyDto.Price);
            scores.Add("رفتار پرسنل ", surveyDto.Behavior);


            var worstscores = scores.Where(s => s.Value == Score.افتضاح || s.Value == Score.بد || s.Value == Score.معمولی);
            var bestscores = scores.Where(s => s.Value == Score.عالی || s.Value == Score.خوب);

            bestscores = bestscores.OrderByDescending(s => s.Value);
            var bests = bestscores.Take(2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            worstscores = worstscores.OrderBy(s => s.Value);
            var worsts = worstscores.Take(2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


            string query1 = "نظر او";
            string query2 = " نظر او";

            if (worsts != null)
            {
                foreach (var item in worsts)
                {
                    query1 += " در مورد " + item.Key + item.Value + " و ";
                }
            }
            if (bests != null)
            {
                foreach (var item in bests)
                {
                    query2 += " در مورد " + item.Key + item.Value + " و ";
                }
                if (query2.EndsWith(" و "))
                {
                    query2 = query2.Substring(0, query2.Length - 3);
                    query2 += " است.";
                }
                if (worsts != null)
                {
                    query1 = query1.Substring(0, query1.Length - 3);
                    query1 += " و همچنین ";
                }
                else query1 = "";
            }
            else
            {
                if (worsts == null)
                {
                    query1 = $"نظر او در مورد رستوران {surveyDto.Overall} است";
                    query2 = "";
                }
                query1 = query1.Substring(0, query1.Length - 3);
                query1 += " است.";
                query2 = "";
            }
            #endregion


            string Query = $"{surveyDto.CustomerName} مشتری رستوران {surveyDto.RestaurantName} در فرم نظر سنجی اعلام کرده از\n";
            Query += query1 + query2;
            Query += $"\nپیامی متنی با دقیقا کمتر از  {surveyDto.LimitSmsTokens} کاراکتر به {surveyDto.CustomerName} با اشاره به نظراتش بنویس.";
            MesssageGPTDto messsageDto = new()
            {
                FaQuestion = Query,
                SurveyId = surveyDto.Id,
                LimitTokens = surveyDto.LimitSmsTokens,
                Temperature = surveyDto.Temperature,
            };
            return messsageDto;
        }

        public MesssageGPTDto CreateCommentSmsMessage(SurveyResultDto surveyDto)
        {


            string Query = $"{surveyDto.CustomerName} مشتری رستوران، در بخش نظر در فرم نظر سنجی گفته: \n{surveyDto.Comment}. \n";

            Query += "در پیامی دقیقا کمتر از 400 کاراکتر ضمن تشکر از او" +
                "در باره نظر او نظر بده";

            //Query += $" پیامی متنی برای او بنویس و ضمن تشکر از شرکت در نظر سنجی، در مورد نظر او نظر بنویس.";
            //Query += "\n همچنین فقط اگر نظر مشتری منفی بود، به او اطلاع بده که";
            //Query += "به عنوان عذرخواهی به مدت یک هفته روی شماره موبایلش تخفیف ویژه 20% ثبت شده";
            //Query += "و اگر نظرش مثبت بود از تخفیف چیزی نگو";
            //Query += "همه اینا رو در دقیقا کمتر از 400 کاراکتر، بگو.";
            MesssageGPTDto messsageDto = new()
            {
                FaQuestion = Query,
                SurveyId = surveyDto.Id,
                LimitTokens = surveyDto.LimitSmsTokens,
                Temperature = surveyDto.Temperature
            };
            return messsageDto;
        }

        public async Task<MesssageGPTDto> CreateHierarchyMessage(SurveyResultDto surveyDto, MesssageGPTDto messageDto)
        {
            string feedString = "chat: this is an interactive chat where you can ask any restaurant-related question.";
            switch (messageDto.Step)
            {
                case MessageStep.First:
                    {
                        #region add scores to query
                        Dictionary<string, Score> scores = new Dictionary<string, Score>(5);
                        scores.Add("کیفیت غذا ", surveyDto.FoodQuality);
                        scores.Add("سطح بهداشت ", surveyDto.Cleanliness);
                        scores.Add("قیمت ", surveyDto.Price);
                        scores.Add("رفتار پرسنل ", surveyDto.Behavior);


                        var worstscores = scores.Where(s => s.Value == Score.افتضاح || s.Value == Score.بد || s.Value == Score.معمولی);
                        var bestscores = scores.Where(s => s.Value == Score.عالی || s.Value == Score.خوب);

                        bestscores = bestscores.OrderByDescending(s => s.Value);
                        var bests = bestscores.Take(2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                        worstscores = worstscores.OrderBy(s => s.Value);
                        var worsts = worstscores.Take(2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


                        string query1 = "نظر او";
                        string query2 = " نظر او";
                        if (worsts.Count > 0)
                        {
                            foreach (var item in worsts)
                            {
                                query1 += " در مورد " + item.Key + item.Value + " و ";
                            }
                        }
                        if (bests.Count > 0)
                        {
                            foreach (var item in bests)
                            {
                                query2 += " در مورد " + item.Key + item.Value + " و ";
                            }
                            if (query2.EndsWith(" و "))
                            {
                                query2 = query2.Substring(0, query2.Length - 3);
                                query2 += " است.";
                            }
                            if (worsts.Count > 0)
                            {
                                query1 += "همچنین ";
                            }
                            else query1 = "";
                        }
                        if (bests.Count == 0)
                        {
                            if (worsts.Count == 0)
                            {
                                query1 = $"نظر او در مورد رستوران {surveyDto.Overall} است";
                                query2 = "";
                            }
                            query1 = query1.Substring(0, query1.Length - 3);
                            query1 += " است.";
                            query2 = "";
                        }
                        #endregion

                        string Query = $"{surveyDto.CustomerName} مشتری رستوران {surveyDto.RestaurantName} فرم نظر سنجی رستوران را پر کرده است.\n";
                        Query += query1 + query2;
                        Query += $"\nبصورت لیست بگو چه مواردی رو قبل از تماس با {surveyDto.CustomerName} باید بدانم و در نظر بگیرم.";

                        messageDto.FaQuestion = Query;
                        messageDto.Question = Query;
                        messageDto.Temperature = surveyDto.Temperature;
                        break;
                    }

                case MessageStep.Second:
                    {

                        string question = "بصورت لیست شماره دار بگو چه مواردی را باید در تماس تلفنی با این مشتری بگویم";
                        string Query = feedString +
                            "\n سوال: " + messageDto.Hierarchy.ElementAt(0).Key + "\n chat: " + messageDto.Hierarchy.ElementAt(0).Value + "." +
                            "\n" + question;
                        messageDto.FaQuestion = Query;
                        messageDto.Question = question;
                        messageDto.Temperature = surveyDto.Temperature;
                        break;
                    }

            }
            messageDto.UsedTokens += feedString.Length;

            return messageDto;
        }

        public async Task<MesssageGPTDto> AddMessage(MesssageGPTDto messageDto)
        {
            try
            {
                var message = _mapper.Map<MesssageGPT>(messageDto);
                if (messageDto.Id == null)
                {
                    await _db.MessageGPTs.AddAsync(message);
                    messageDto.Id = message.Id;
                }
                if (messageDto.IsManualWithContext)
                {
                    Answer answer = new()
                    {
                        AnswerString = messageDto.RawAnswer,
                        Length = messageDto.RawAnswer.Length

                    };
                    await _db.Answers.AddAsync(answer);
                    Question question = new()
                    {
                        QuestinString = messageDto.Question,
                        Length = messageDto.Question.Length,
                        Answer = answer,
                        AnswerId = answer.Id,
                        Message = await _db.MessageGPTs.FirstOrDefaultAsync(m => m.Id == messageDto.Id),
                        messageId = message.Id
                    };
                    await _db.Questions.AddAsync(question);
                    await _db.SaveChangesAsync();
                }
                if (messageDto.Hierarchy != null)
                {
                    int i = 0;
                    foreach (var item in messageDto.Hierarchy)
                    {
                        i++;
                        Answer answer = new()
                        {
                            AnswerString = item.Value,
                            Step = i,
                            Length = item.Value.Length
                        };
                        await _db.Answers.AddAsync(answer);
                        Question question = new()
                        {
                            QuestinString = item.Key,
                            Length = item.Key.Length,
                            Answer = answer,
                            AnswerId = answer.Id,
                            Step = i,
                            Message = message,
                            messageId = messageDto.Id
                        };
                        await _db.Questions.AddAsync(question);

                        await _db.SaveChangesAsync();
                    }
                }

                await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                messageDto.ErrorMessage = ex.Message;
                messageDto.IsSuccess = false;
            }
            return messageDto;
        }

        public async Task<MesssageGPTDto> AskManualQuestion(MesssageGPTDto messageDto)
        {
            if (messageDto.DoTranslate)
            {
                messageDto.IsAsked = false;
                messageDto.Type = MessageType.Translated;
                messageDto = await AskTranslatedMessage(messageDto);
            }
            else
            {
                messageDto.Type = MessageType.Simple;
                messageDto = await AskSimpleMessage(messageDto);
            }
            //messageDto = await AddMessage(messageDto);

            return messageDto;
        }

        public async Task<bool> RemoveMessage(Guid id)
        {
            var message = await _db.MessageGPTs.FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
                return false;
            message.IsRemoved = true;
            int res = await _db.SaveChangesAsync();
            if (res == 0)
                return false;
            else return true;
        }

        public async Task<bool> DeleteMessage(Guid messageId)
        {
            MesssageGPT? message = await _db.MessageGPTs.Include(m=>m.Questions).ThenInclude(q=>q.Answer).Where(m => m.Id == messageId).FirstAsync();
            if (message == null) return false;
            if(message.Questions.Count() != 0)
            {
                foreach (var question in message.Questions)
                {
                    if(question.Answer!=null)
                        _db.Answers.Remove(question.Answer);
                    _db.Questions.Remove(question);
                }
            }
            var res = _db.MessageGPTs.Remove(message);
            if (res.State == 0) return false;
            await _db.SaveChangesAsync();            
            return true;
        }

        public async Task<MesssageGPTDto> AskManualQuestionWithContext(MesssageGPTDto messageDto)
        {
            string? Prompt = "";
            string? questionString;


            if (messageDto.Id == null)
            {
                messageDto.Question = messageDto.FaQuestion;
                messageDto.LimitTokens = messageDto.FaQuestion.Length + 500;
                if (messageDto.DoTranslate)
                    messageDto = await AskTranslatedMessage(messageDto);
                else
                    messageDto = await AskSimpleMessage(messageDto);

                messageDto = await AddMessage(messageDto);
            }
            else
            {
                int length = messageDto.FaQuestion.Length;
                questionString = messageDto.FaQuestion;
                messageDto = _mapper.Map<MesssageGPTDto>(await _db.MessageGPTs.Include(m => m.Questions.OrderByDescending(q => q.InsertTime)).ThenInclude(q => q.Answer).FirstOrDefaultAsync(m => m.Id == messageDto.Id));
                messageDto.FaQuestion = questionString;
                int count = messageDto.Questions.Count;
                string lastQuestion;


                foreach (var quest in messageDto.Questions)
                {
                    length += quest.QuestinString.Length + quest.Answer.AnswerString.Length;
                    if (length < 3600)
                    {
                        lastQuestion = "سوال: " + quest.QuestinString + "\n" + "chat: " + quest.Answer.AnswerString + "\n";
                        Prompt = lastQuestion + Prompt;
                    }
                    else
                    {
                        break;
                    }

                }
                if (string.IsNullOrEmpty(messageDto.Subject))
                    Prompt = "chat: this is an interactive chat where you can ask any question.\n" + Prompt;
                else Prompt = "chat: this is an interactive chat where you can ask any question about " + messageDto.Subject + "\n" + Prompt;
                Prompt += messageDto.FaQuestion;
                length += Prompt.Length + 450;
                messageDto.Question = messageDto.FaQuestion;
                messageDto.FaQuestion = Prompt;
                messageDto.LimitTokens = length;
                if (messageDto.DoTranslate)
                    messageDto = await AskTranslatedMessage(messageDto);
                else
                    messageDto = await AskSimpleMessage(messageDto);

                messageDto = await AddMessage(messageDto);
            }

            return messageDto;
        }

        public async Task<MesssageGPTDto> GetMessage(Guid? messageId)
        {
            if (messageId == null)
            {
                MesssageGPTDto messsageDto = new()
                {
                    IsSuccess = false
                };

                return messsageDto;
            }
            else
            {
                return _mapper.Map<MesssageGPTDto>(await _db.MessageGPTs.Include(m => m.Questions.OrderByDescending(q => q.InsertTime)).ThenInclude(q => q.Answer).FirstOrDefaultAsync(m => m.Id == messageId));
            }
        }

        public async Task<bool> RemoveQuestion(Guid QuestionId)
        {
            Question? question = await _db.Questions.Include(q=>q.Answer).Where(Q => Q.Id == QuestionId).FirstAsync();
            if (question == null) return false;
            var res = _db.Answers.Remove(question.Answer);
            var res2 = _db.Questions.Remove(question);
            if (res.State == 0 || res2.State == 0) return false;
            await _db.SaveChangesAsync();
            
            return true;
        }
    }
}

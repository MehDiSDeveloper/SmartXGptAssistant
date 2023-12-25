using Newtonsoft.Json.Linq;
using SmartXGPT.Service.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartXGPT.Service.Interfaces;


namespace SmartXGPT.Infrastructure.APIServices
{
    public class TranslationService:ITranslationService
    {
        public async Task<MesssageGPTDto> TrMessage(MesssageGPTDto messageDto)
        {

            var client = new HttpClient();
            MesssageGPTDto response = messageDto;
            string query = "";
            string fromCode = "en";
            string toCode = "fa";
            if (messageDto.IsAsked)
            {
                query = messageDto.EnAnswer;
            }
            else
            {
                query = messageDto.FaQuestion;
                fromCode = "fa";
                toCode = "en";
            }
            Uri uri = new Uri("https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + fromCode + "&tl=" + toCode + "&dt=t&q=" + System.Web.HttpUtility.UrlEncode(query));
            var result = await client.GetAsync(uri);
            System.Diagnostics.Debug.WriteLine(uri.ToString());
            string json = await result.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(json))
            {
                response.ErrorMessage = "مشکلی با سرویس ترجمه گوگل رخ داده";
                response.IsSuccess = false;
            }
            else if (json.StartsWith("<html")|| json.StartsWith("<!DOCTYPE"))
            {
                response.ErrorMessage = "مشکلی در url شما برای ترجمه از گوگل رخ داده. Request URL:\n" + uri.ToString();
                response.IsSuccess = false;
            }
            else
            {
                JArray jArray = JArray.Parse(json);
                string generatedTranslation = string.Empty;
                foreach (JToken item in jArray.First())
                {
                    if (item.Type == JTokenType.Array && item[0].Type == JTokenType.String)
                    {
                        string translation = item[0].ToString();
                        generatedTranslation += translation + " ";
                    }
                }
                if (generatedTranslation.EndsWith(" "))
                {
                    generatedTranslation = generatedTranslation.Substring(0, generatedTranslation.Length - 1);
                    generatedTranslation = generatedTranslation.Replace("  ", " ");
                }
                else
                {
                    generatedTranslation = generatedTranslation.Replace("  ", " ");
                }
                if (messageDto.IsAsked)
                {
                    response.FaAnswer = generatedTranslation;
                }
                else
                {
                    response.EnQuestion = generatedTranslation;
                }
                response.IsSuccess = true;
            }

            return response;
        }
    }
}

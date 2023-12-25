using SmartXGPT.Service.Models.Commons;
using SmartXGPT.Service.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Models.DTOs
{
    public class MesssageGPTDto
    {
        [Key]
        public Guid? Id { get; set; }
        public Guid? SurveyId { get; set; }
        public string? AskThis { get; set; }
        public string? RawAnswer { get; set; }
        public string? Question { get; set; }
        public string FaQuestion { get; set; } = "";
        public string? EnQuestion { get; set; }
        public string? EnAnswer { get; set; }
        public string? FaAnswer { get; set; }
        public string? Sms { get; set; }
        public string? Subject { get; set; }
        public Dictionary<string, string>? Hierarchy { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsAsked { get; set; }
        public bool DoTranslate { get; set; }
        public bool IsManual { get; set; }
        public bool IsManualWithContext { get; set; }
        public double Temperature { get; set; } = 0.8;
        public int LimitTokens { get; set; } = 4000;
        public int UsedTokens { get; set; } = 0;
        public MessageStep Step { get; set; } = 0;
        public MessageType Type { get; set; } = 0;

        //nav props
        public SurveyResult? Survey { get; set; }
        public ICollection<Question>? Questions { get; set; }

    }

}
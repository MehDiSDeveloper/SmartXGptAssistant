using SmartXGPT.Service.Models.Commons;
using SmartXGPT.Service.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Models.Entities
{
    public class MesssageGPT:BaseEntity
    {
        public MesssageGPT()
        {
            Id = Guid.NewGuid();            
            DoTranslate = false;
            IsSuccess = false;
            IsAsked = false;    
            IsManual = false;
            IsManualWithContext = false;
        }
        public Guid? SurveyId { get; set; }
        public string? AskThis { get; set; }
        public string? RawAnswer { get; set; }
        public string? FaQuestion { get; set; }
        public string? EnQuestion { get; set; }
        public string? EnAnswer { get; set; }
        public string? FaAnswer { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Subject { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsAsked { get; set; }
        public bool DoTranslate { get; set; }
        public bool IsManual { get; set; }
        public bool IsManualWithContext { get; set; }
        public double Temperature { get; set; } = 0.8;
        public MessageStep Step { get; set; } = 0;
        public MessageType Type { get; set; } = 0;
        public int? UsedTokens { get; set; }

        //nav props
        public SurveyResult Survey { get; set; }
        public ICollection<Question>? Questions { get; set; }

    }
    public enum MessageType
    {
        None = 0,
        Simple = 1,
        Translated = 2,
        TranslatedHierarchy = 3,
        sms= 4,
    };
    public enum MessageStep
    {
        First = 1,
        Second = 2,
    }
}

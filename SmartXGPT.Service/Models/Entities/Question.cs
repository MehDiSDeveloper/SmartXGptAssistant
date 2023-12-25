using SmartXGPT.Service.Models.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Models.Entities
{
    public class Question : BaseEntity
    {
        public Question()
        {
            Id = Guid.NewGuid();
        }
        public string QuestinString { get; set; }
        public int? Step { get; set; }
        public int? Length { get; set; }
        public Guid? AnswerId { get; set; }
        public Guid? messageId { get; set; }
        public Answer? Answer { get; set; }
        public  MesssageGPT Message { get; set; }


    }
}

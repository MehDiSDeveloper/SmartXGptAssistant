using SmartXGPT.Service.Models.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Models.Entities
{
    public class Answer : BaseEntity
    {
        public Answer()
        {
            Id = Guid.NewGuid();
        }
        public string AnswerString { get; set; }
        public int? Step { get; set; }
        public int? Length { get; set; }
    }
}

using System.Xml.Linq;

namespace SmartXGPT.Service.Models.DTOs
{
    public class AssistantReqDto
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string ThreadId { get; set; } = string.Empty;
    }
}

using SmartXGPT.Service.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Interfaces
{
    public interface IGPTAPIService
    {
        Task<MesssageGPTDto> AskGPTAsync(MesssageGPTDto messageDto);
        //Task<MesssageGPTDtoDto> TreatMessage(MesssageGPTDtoDto messageDto);
        Task<AssistantReqDto> AskGPTAssistantAsync(AssistantReqDto assistantReqDto);
    }
}

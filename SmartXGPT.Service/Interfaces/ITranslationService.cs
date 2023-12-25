using SmartXGPT.Service.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Interfaces
{
    public interface ITranslationService
    {
        Task<MesssageGPTDto> TrMessage(MesssageGPTDto messageDto);

    }
}

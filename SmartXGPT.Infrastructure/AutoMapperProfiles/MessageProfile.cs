using AutoMapper;
using SmartXGPT.Service.Models.DTOs;
using SmartXGPT.Service.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Infrastructure.AutoMapperProfiles
{
    public class MessageProfile:Profile
    {
        public MessageProfile()
        {
            CreateMap<MesssageGPT,MesssageGPTDto>().ReverseMap();
        }
    }
}

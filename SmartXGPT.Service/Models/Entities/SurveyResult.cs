using SmartXGPT.Service.Models.Commons;
using SmartXGPT.Service.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Models.Entities
{
    public class SurveyResult : BaseEntity
    {
        public SurveyResult()
        {
            Id = Guid.NewGuid();
            CustomerName = string.Empty;
            RestaurantName = string.Empty;
            Comment = string.Empty;
        }
        public string CustomerName { get; set; }
        public string RestaurantName { get; set; }
        public string? Comment { get; set; }
        public double Temperature { get; set; } = 1.0;
        public Score FoodQuality { get; set; }
        public Score Cleanliness { get; set; }
        public Score Price { get; set; }
        public Score Behavior { get; set; }
        public Score Overall { get; set; }
        public bool DoTranslate { get; set; }
        public bool DoSimple { get; set; }
        public bool DoHierarchy { get; set; }
        public bool DoSms { get; set; }

        public bool DoCommentSms { get; set; }
        //nav props

        public List<MesssageGPT>? Messages { get; set; }
    }
    public enum Score
    {
        ممتنع = 0,
        افتضاح = 1,
        بد = 2,
        معمولی = 3,
        خوب = 4,
        عالی = 5,
    }
    
}

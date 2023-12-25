using SmartXGPT.Service.Models.Commons;
using SmartXGPT.Service.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Service.Models.DTOs
{
    public class SurveyResultDto
    {
        [Key]
        public Guid? Id { get; set; }
        public Guid CustomerId { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "لطفا نام مشتری را وارد کنید")]
        public string CustomerName { get; set; } = "";
        [Required(ErrorMessage = "لطفا نام رستوران را وارد کنید")]
        public string RestaurantName { get; set; }
        [MaxLength(250, ErrorMessage = "طول نظر بیش از 250 کاراکتر شده است")]
        public string? Comment { get; set; }
        public string? ErrorMessage { get; set; } = "";
        [Range(0.0, 2.0, ErrorMessage = "مقدار بایستی بین 0.0 تا 2.0 باشد")]
        public double Temperature { get; set; } = 0.1;
        public bool IsSuccess { get; set; }
        [DisplayName("کیفیت غذا")]
        public Score FoodQuality { get; set; }
        [DisplayName("بهداشت")]
        public Score Cleanliness { get; set; }
        [DisplayName("قیمت")]
        public Score Price { get; set; }
        [DisplayName("رفتار پرسنل")]
        public Score Behavior { get; set; }
        [DisplayName("در مجموع")]
        [Required(ErrorMessage = "این فیلد الزامی است")]
        public Score Overall { get; set; }
        public bool DoTranslate { get; set; } = false;
        public bool DoSimple { get; set; } = false;
        public bool DoHierarchy { get; set; } = false;
        public bool DoSms { get; set; } = false;
        public bool DoCommentSms { get; set; } = false;
        public int LimitTokens { get; set; } = 4000;
        public int LimitSmsTokens { get; set; } = 300;

        //nav props

        public List<MesssageGPT>? Messages { get; set; }

    }

}

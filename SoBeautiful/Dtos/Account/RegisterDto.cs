using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SoBeautiful.Entities.Enums;

namespace SoBeautiful.Dtos.Account
{
    public class RegisterDto
    {
        [JsonProperty(nameof(UserName))]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; }

        [JsonProperty(nameof(Password))]
        [Display(Name = "密碼")]
        public string Password { get; set; }
        
        [JsonProperty(nameof(PasswordConfirm))]
        [Display(Name = "確認密碼")]
        public string PasswordConfirm { get; set; }
        
        [JsonProperty(nameof(Email))]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }
        
        [JsonProperty(nameof(Surname))]
        [Display(Name = "姓氏")]
        public string Surname { get; set; }
        
        [JsonProperty(nameof(GivenName))]
        [Display(Name = "名字")]
        public string GivenName { get; set; }
        
        [JsonProperty(nameof(Gender))]
        [Display(Name = "性別")]
        public GenderType Gender { get; set; }
        
        [JsonProperty(nameof(DateOfBirth))]
        [Display(Name = "生日")]
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
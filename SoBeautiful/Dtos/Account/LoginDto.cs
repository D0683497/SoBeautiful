using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SoBeautiful.Dtos.Account
{
    public class LoginDto
    {
        [JsonProperty(nameof(UserName))]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; }
        
        [JsonProperty(nameof(Password))]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }
}
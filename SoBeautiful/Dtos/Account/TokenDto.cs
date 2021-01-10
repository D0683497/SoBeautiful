using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SoBeautiful.Dtos.Account
{
    public class TokenDto
    {
        [JsonProperty(nameof(AccessToken))]
        [Display(Name = "存取權杖")]
        public string AccessToken { get; set; }
    }
}
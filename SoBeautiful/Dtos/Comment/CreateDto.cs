using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SoBeautiful.Dtos.Comment
{
    public class CreateDto
    {
        [JsonProperty(nameof(Content))]
        [Display(Name = "內容")]
        public string Content { get; set; }
    }
}
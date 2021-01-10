using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SoBeautiful.Dtos.Article
{
    public class CreateDto
    {
        [JsonProperty(nameof(Title))]
        [Display(Name = "標題")]
        public string Title { get; set; }

        [JsonProperty(nameof(Content))]
        [Display(Name = "內容")]
        public string Content { get; set; }
    }
}
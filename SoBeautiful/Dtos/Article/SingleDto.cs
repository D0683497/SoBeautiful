using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SoBeautiful.Dtos.Article
{
    public class SingleDto
    {
        [JsonProperty(nameof(ArticleId))]
        [Display(Name = "文章識別碼")]
        public string ArticleId { get; set; }

        [JsonProperty(nameof(Title))]
        [Display(Name = "標題")]
        public string Title { get; set; }

        [JsonProperty(nameof(Content))]
        [Display(Name = "內容")]
        public string Content { get; set; }

        [JsonProperty(nameof(Like))]
        [Display(Name = "按讚數")]
        public int Like { get; set; }

        [JsonProperty(nameof(Dislike))]
        [Display(Name = "倒讚數")]
        public int Dislike { get; set; }
    }
}
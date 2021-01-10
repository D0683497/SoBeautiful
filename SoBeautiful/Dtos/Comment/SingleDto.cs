using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SoBeautiful.Dtos.Comment
{
    public class SingleDto
    {
        [JsonProperty(nameof(CommentId))]
        [Display(Name = "留言識別碼")]
        public string CommentId { get; set; }
        
        [JsonProperty(nameof(Content))]
        [Display(Name = "內容")]
        public string Content { get; set; }
    }
}
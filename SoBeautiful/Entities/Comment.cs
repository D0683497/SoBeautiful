using System;
using System.ComponentModel.DataAnnotations;

namespace SoBeautiful.Entities
{
    /// <summary>
    /// 留言
    /// </summary>
    public class Comment
    {
        [Key]
        public string CommentId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        
        public string ArticleId { get; set; }
        public Article Article { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
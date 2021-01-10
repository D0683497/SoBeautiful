using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoBeautiful.Entities
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Article
    {
        [Key]
        public string ArticleId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 按讚數
        /// </summary>
        public int Like { get; set; } = 0;

        /// <summary>
        /// 倒讚數
        /// </summary>
        public int Dislike { get; set; } = 0;

        public ICollection<Comment> Comments { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
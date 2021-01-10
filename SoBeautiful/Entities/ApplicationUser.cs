using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SoBeautiful.Entities.Enums;

namespace SoBeautiful.Entities
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 是否啟用帳戶
        /// </summary>
        public bool IsEnable { get; set; } = false;
        
        /// <summary>
        /// 姓氏
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string GivenName { get; set; }
        
        /// <summary>
        /// 性別
        /// </summary>
        public GenderType Gender { get; set; }
        
        /// <summary>
        /// 生日
        /// </summary>
        public DateTimeOffset DateOfBirth { get; set; }

        public ICollection<Article> Articles { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
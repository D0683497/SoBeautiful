using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoBeautiful.Entities;

namespace SoBeautiful.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region 日期儲存
            
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
                // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
                // use the DateTimeOffsetToBinaryConverter
                // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
                // This only supports millisecond precision, but should be sufficient for most use cases.
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                        || p.PropertyType == typeof(DateTimeOffset?));
                    foreach (var property in properties)
                    {
                        builder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }

            #endregion

            #region User

            builder.Entity<ApplicationUser>()
                .Property(x => x.IsEnable)
                .IsRequired();
            
            builder.Entity<ApplicationUser>()
                .Property(x => x.Surname)
                .HasMaxLength(20)
                .IsRequired();
            
            builder.Entity<ApplicationUser>()
                .Property(x => x.GivenName)
                .HasMaxLength(20)
                .IsRequired();
            
            builder.Entity<ApplicationUser>()
                .Property(x => x.Gender)
                .IsRequired();
            
            builder.Entity<ApplicationUser>()
                .Property(x => x.DateOfBirth)
                .IsRequired();

            #endregion

            #region Article

            builder.Entity<Article>()
                .Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Entity<Article>()
                .Property(x => x.Content)
                .IsRequired();
            
            builder.Entity<Article>()
                .Property(x => x.Like)
                .IsRequired();
            
            builder.Entity<Article>()
                .Property(x => x.Dislike)
                .IsRequired();

            #endregion

            #region Comment

            builder.Entity<Comment>()
                .Property(x => x.Content)
                .HasMaxLength(500)
                .IsRequired();

            #endregion

            #region ApplicationUser 跟 Article 一對多

            builder.Entity<Article>()
                .HasOne(article => article.User)
                .WithMany(user => user.Articles)
                .HasForeignKey(article => article.UserId);

            #endregion
            
            #region ApplicationUser 跟 Comment 一對多

            builder.Entity<Comment>()
                .HasOne(comment => comment.User)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.UserId);

            #endregion
            
            #region Article 跟 Comment 一對多

            builder.Entity<Comment>()
                .HasOne(comment => comment.Article)
                .WithMany(article => article.Comments)
                .HasForeignKey(comment => comment.ArticleId);

            #endregion
        }
    }
}
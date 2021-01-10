using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoBeautiful.Data;
using SoBeautiful.Entities;
using SoBeautiful.Parameters;

namespace SoBeautiful.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public ArticlesController(
            ILogger<ArticlesController> logger,
            IMapper mapper, 
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost(Name = nameof(CreateArticle))]
        public async Task<IActionResult> CreateArticle(SoBeautiful.Dtos.Article.CreateDto model)
        {
            #region 獲取使用者 Id

            var userId = User.Claims
                .SingleOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            #endregion
            
            SoBeautiful.Validators.Article.CreateDtoValidator validator = new SoBeautiful.Validators.Article.CreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                var entity = _mapper.Map<Article>(model);
                entity.UserId = userId;

                try
                {
                    await _dbContext.Articles.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    
                    return Ok();
                }
                catch (DbUpdateException)
                {
                    result.Errors.Add(new ValidationFailure("", "建立失敗"));
                }
            }
            
            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(GetArticles))]
        public async Task<ActionResult<IEnumerable<SoBeautiful.Dtos.Article.SingleDto>>> GetArticles([FromQuery] PaginationResourceParameters parameters)
        {
            #region 獲取資料
        
            var entities = await _dbContext.Articles
                .AsNoTracking()
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        
            var models = _mapper.Map<IEnumerable<SoBeautiful.Dtos.Article.SingleDto>>(entities);
        
            #endregion
            
            #region 分頁資訊
        
            var length = await _dbContext.Articles
                .CountAsync();
            
            var paginationMetadata = new
            {
                pageLength = length, // 總資料數
                pageSize = parameters.PageSize, // 一頁的項目數
                pageIndex = parameters.PageIndex, // 目前頁碼
            };
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        
            #endregion
            
            return Ok(models);
        }
        
        [AllowAnonymous]
        [HttpGet("{articleId}", Name = nameof(GetArticle))]
        public async Task<ActionResult<SoBeautiful.Dtos.Article.SingleDto>> GetArticle(string articleId)
        {
            #region 文章是否存在

            var isExist = await _dbContext.Articles
                .AnyAsync(x => x.ArticleId == articleId);
            if (!isExist)
            {
                return NotFound();
            }

            #endregion

            #region 獲取資料

            var entity = await _dbContext.Articles
                .AsNoTracking()
                .Where(x => x.ArticleId == articleId)
                .SingleOrDefaultAsync();

            var models = _mapper.Map<SoBeautiful.Dtos.Article.SingleDto>(entity);

            #endregion
            
            return Ok(models);
        }
        
        [Authorize]
        [HttpGet("{articleId}/comments", Name = nameof(GetComments))]
        public async Task<ActionResult<IEnumerable<SoBeautiful.Dtos.Comment.SingleDto>>> GetComments(string articleId, [FromQuery] PaginationResourceParameters parameters)
        {
            #region 文章是否存在

            var isExist = await _dbContext.Articles
                .AnyAsync(x => x.ArticleId == articleId);
            if (!isExist)
            {
                return NotFound();
            }

            #endregion
            
            #region 獲取資料
        
            var entities = await _dbContext.Comments
                .AsNoTracking()
                .Where(x => x.ArticleId == articleId)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        
            var models = _mapper.Map<IEnumerable<SoBeautiful.Dtos.Comment.SingleDto>>(entities);
        
            #endregion
            
            #region 分頁資訊
        
            var length = await _dbContext.Comments
                .Where(x => x.ArticleId == articleId)
                .CountAsync();
            
            var paginationMetadata = new
            {
                pageLength = length, // 總資料數
                pageSize = parameters.PageSize, // 一頁的項目數
                pageIndex = parameters.PageIndex, // 目前頁碼
            };
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        
            #endregion
            
            return Ok(models);
        }
        
        [HttpPost("{articleId}/comments", Name = nameof(CreateComment))]
        public async Task<IActionResult> CreateComment(string articleId, SoBeautiful.Dtos.Comment.CreateDto model)
        {
            #region 文章是否存在

            var isExist = await _dbContext.Articles
                .AnyAsync(x => x.ArticleId == articleId);
            if (!isExist)
            {
                return NotFound();
            }

            #endregion
            
            #region 獲取使用者 Id

            var userId = User.Claims
                .SingleOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            #endregion
            
            SoBeautiful.Validators.Comment.CreateDtoValidator validator = new SoBeautiful.Validators.Comment.CreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                var entity = _mapper.Map<Comment>(model);
                entity.UserId = userId;
                entity.ArticleId = articleId;

                try
                {
                    await _dbContext.Comments.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    
                    return Ok();
                }
                catch (DbUpdateException)
                {
                    result.Errors.Add(new ValidationFailure("", "建立失敗"));
                }
            }
            
            return BadRequest(result.Errors);
        }
    }
}
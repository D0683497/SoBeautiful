using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SoBeautiful.Data;
using SoBeautiful.Dtos.Account;
using SoBeautiful.Entities;
using SoBeautiful.Validators.Account;

namespace SoBeautiful.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            ILogger<AccountController> logger, 
            IMapper mapper,
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        
        [AllowAnonymous]
        [HttpPost("login", Name = nameof(Login))]
        public async Task<ActionResult<TokenDto>> Login(LoginDto model)
        {
            LoginDtoValidator validator = new LoginDtoValidator(_userManager, _signInManager);
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                var token = await GenerateJwtToken(model.UserName);
                return Ok(token);
            }
            
            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("register", Name = nameof(Register))]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            RegisterDtoValidator validator = new RegisterDtoValidator(_userManager);
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(model);
                
                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        #region 建立使用者

                        if (await _userManager.CreateAsync(user, model.Password) != IdentityResult.Success)
                        {
                            throw new DbUpdateException("建立使用者失敗");
                        }
    
                        #endregion
                        
                        await transaction.CommitAsync();
                        return NoContent();
                    }
                    catch (DbUpdateException)
                    {
                        await transaction.RollbackAsync();
                        result.Errors.Add(new ValidationFailure("", "註冊失敗"));
                    }
                }
            }
            
            return BadRequest(result.Errors);
        }
        
        #region 產生 Token

        private async Task<TokenDto> GenerateJwtToken(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);

            #region 添加角色聲明

            var roleNames = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }

            #endregion
            
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            
            var userClaimsIdentity = new ClaimsIdentity(claims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            // 正式環境要用 SecurityAlgorithms.HmacSha256Signature
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                Subject = userClaimsIdentity,
                NotBefore = DateTime.Now, // Token 在什麼時間之前，不可用
                IssuedAt = DateTime.UtcNow, // Token 的建立時間
                Expires = DateTime.Now.AddHours(2), // Token 的逾期時間
                SigningCredentials = signingCredentials
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return new TokenDto { AccessToken = serializeToken };
        }

        #endregion
    }
}
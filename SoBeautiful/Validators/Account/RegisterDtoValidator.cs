using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoBeautiful.Dtos.Account;
using SoBeautiful.Entities;

namespace SoBeautiful.Validators.Account
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterDtoValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            
            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("使用者名稱")
                .WithMessage("{PropertyName}是必填的")
                .MaximumLength(256)
                .WithName("使用者名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .Matches(@"^.[\w\-\.\@\+\#\$\%\\\/\(\)\[\]\*\&\:\>\<\^\!\{\}\=]+$")
                .WithName("使用者名稱")
                .WithMessage("{PropertyName}只能是字母或數字或 - . _ @ + # $ % \\ / ( ) [ ] * & : > < ^ ! {{ }} =")
                .Custom(async (parameters, context) =>
                {
                    if (await ExistUserName(parameters))
                    {
                        context.AddFailure("使用者名稱已經被使用");
                    }
                });
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithName("密碼")
                .WithMessage("{PropertyName}是必填的")
                .Length(8, 64)
                .WithName("密碼")
                .WithMessage("{PropertyName}長度需介於{MinLength}到{MaxLength}之間");
            RuleFor(x => x.PasswordConfirm)
                .NotEmpty()
                .WithName("確認密碼")
                .WithMessage("{PropertyName}是必填的")
                .Length(8, 64)
                .WithName("確認密碼")
                .WithMessage("{PropertyName}長度需介於{MinLength}到{MaxLength}之間")
                .Equal(x => x.Password)
                .WithName("確認密碼")
                .WithMessage("{PropertyName}與密碼不相同");
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("電子郵件")
                .WithMessage("{PropertyName}是必填的")
                .EmailAddress()
                .WithName("電子郵件")
                .WithMessage("{PropertyName}格式錯誤")
                .MaximumLength(256)
                .WithName("電子郵件")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .Custom(async (parameters, context) =>
                {
                    if (await ExistEmail(parameters))
                    {
                        context.AddFailure("電子郵件已經被使用");
                    }
                });
            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithName("姓氏")
                .WithMessage("{PropertyName}是必填的")
                .MaximumLength(20)
                .WithName("姓氏")
                .WithMessage("{PropertyName}最多{MaxLength}");
            RuleFor(x => x.GivenName)
                .NotEmpty()
                .WithName("名字")
                .WithMessage("{PropertyName}是必填的")
                .MaximumLength(20)
                .WithName("名字")
                .WithMessage("{PropertyName}最多{MaxLength}");
            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithName("性別")
                .WithMessage("{PropertyName}是必填的")
                .IsInEnum()
                .WithName("性別")
                .WithMessage("{PropertyName}格式錯誤");
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithName("生日")
                .WithMessage("{PropertyName}是必填的")
                .LessThan(DateTimeOffset.Now)
                .WithName("生日")
                .WithMessage("{PropertyName}不能晚於今天");
        }
        
        private async Task<bool> ExistUserName(string userName)
        {
            // true 使用者名稱已經被使用
            return await _userManager.Users
                .AnyAsync(x => x.NormalizedUserName == userName.ToUpper());
        }

        private async Task<bool> ExistEmail(string email)
        {
            // true 電子郵件已經被使用
            return await _userManager.Users
                .AnyAsync(x => x.NormalizedEmail == email.ToUpper());
        }
    }
}
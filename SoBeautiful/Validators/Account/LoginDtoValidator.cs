using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SoBeautiful.Dtos.Account;
using SoBeautiful.Entities;

namespace SoBeautiful.Validators.Account
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginDtoValidator(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
            bool validationSuccess = true;
            
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithName("使用者名稱")
                .WithMessage("{PropertyName}是必填的")
                .MaximumLength(256)
                .WithName("使用者名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .Matches(@"^.[\w\-\.\@\+\#\$\%\\\/\(\)\[\]\*\&\:\>\<\^\!\{\}\=]+$")
                .WithName("使用者名稱")
                .WithMessage("{PropertyName}只能是字母或數字或 - . _ @ + # $ % \\ / ( ) [ ] * & : > < ^ ! {{ }} =")
                .OnAnyFailure(x => { validationSuccess = false; });
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithName("密碼")
                .WithMessage("{PropertyName}是必填的")
                .Length(8, 64)
                .WithName("密碼")
                .WithMessage("{PropertyName}長度需介於{MinLength}到{MaxLength}之間")
                .OnAnyFailure(x => { validationSuccess = false; });
            
            When(x => validationSuccess, () =>
            {
                RuleFor(x => new {x.UserName, x.Password})
                    .Custom(async (parameters, context) =>
                    {
                        switch (await Login(parameters.UserName, parameters.Password))
                        {
                            case LoginStatus.Succeeded:
                                break;
                            case LoginStatus.IsLockedOut:
                                context.AddFailure("帳戶被鎖定");
                                break;
                            case LoginStatus.IsNotAllowed:
                                context.AddFailure("帳戶尚未驗證");
                                break;
                            default:
                                context.AddFailure("登入失敗");
                                break;
                        }
                    });
            });
        }
        
        private async Task<LoginStatus> Login(string userName, string password)
        {
            // 獲取使用者
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return LoginStatus.Fail;
            }
            if (!user.IsEnable)
            {
                return LoginStatus.IsNotAllowed;
            }
            
            // 檢查使用者密碼
            var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);
            if (checkPasswordResult.Succeeded)
            {
                return LoginStatus.Succeeded;
            }
            if (checkPasswordResult.IsLockedOut)
            {
                return LoginStatus.IsLockedOut;
            }
            if (checkPasswordResult.IsNotAllowed)
            {
                return LoginStatus.IsNotAllowed;
            }

            return LoginStatus.Fail;
        }

        private enum LoginStatus
        {
            IsNotAllowed, // 未驗證、審核過
            IsLockedOut, // 被封鎖
            Succeeded,
            Fail
        }
    }
}
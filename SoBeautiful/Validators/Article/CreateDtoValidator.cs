using FluentValidation;
using SoBeautiful.Dtos.Article;

namespace SoBeautiful.Validators.Article
{
    public class CreateDtoValidator : AbstractValidator<CreateDto>
    {
        public CreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithName("標題")
                .WithMessage("{PropertyName}是必填的")
                .MaximumLength(50)
                .WithName("標題")
                .WithMessage("{PropertyName}最多{MaxLength}");
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithName("內容")
                .WithMessage("{PropertyName}是必填的");
        }
    }
}
using FluentValidation;
using SoBeautiful.Dtos.Comment;

namespace SoBeautiful.Validators.Comment
{
    public class CreateDtoValidator : AbstractValidator<CreateDto>
    {
        public CreateDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithName("內容")
                .WithMessage("{PropertyName}是必填的")
                .MaximumLength(500)
                .WithName("標題")
                .WithMessage("{PropertyName}最多{MaxLength}");
        }
    }
}
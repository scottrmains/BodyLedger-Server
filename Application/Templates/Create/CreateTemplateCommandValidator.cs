using FluentValidation;

namespace Application.Templates.Create;

internal sealed class CreateTemplateCommandValidator : AbstractValidator<CreateTemplateCommand>
{
    public CreateTemplateCommandValidator()
    {
        RuleFor(c => c.Description).MaximumLength(400);
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
    }
}

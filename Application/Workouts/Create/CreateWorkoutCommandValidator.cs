using FluentValidation;

namespace Application.Workouts.Create;

internal sealed class CreateWorkoutTemplateCommandValidator : AbstractValidator<CreateWorkoutTemplateCommand>
{
    public CreateWorkoutTemplateCommandValidator()
    {
        RuleFor(c => c.Description).MaximumLength(400);
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
    }
}

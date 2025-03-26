using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.Enums;


namespace Application.Templates.Create
{
    internal sealed class CreateWorkoutTemplateCommandHandler(IApplicationDbContext context)
        : ICommandHandler<CreateWorkoutTemplateCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateWorkoutTemplateCommand command, CancellationToken cancellationToken)
        {
            if (await context.Templates.AnyAsync(t => t.Name == command.Name && t.UserId == command.UserId, cancellationToken))
            {
                return Result.Failure<Guid>(TemplateErrors.TemplateNameNotUnique);
            }

            var activities = command.Activities.Select(e => new WorkoutActivity
            {
                ActivityName = e.ActivityName,
                RecommendedSets = e.RecommendedSets,
                RepRanges = e.RepRanges
            }).ToList();

            var workout = new WorkoutTemplate
            {
                Name = command.Name,
                Description = command.Description,
                UserId = command.UserId,
                Activities = activities
            };

            workout.Raise(new TemplateCreatedDomainEvent(workout.Id, TemplateType.Workout));

            context.WorkoutTemplates.Add(workout);
            await context.SaveChangesAsync(cancellationToken);

            return workout.Id;
        }
    }
}
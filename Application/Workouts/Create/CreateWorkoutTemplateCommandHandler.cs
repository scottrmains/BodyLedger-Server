using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Domain.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SharedKernel;

namespace Application.Workouts.Create
{
    internal sealed class CreateWorkoutTemplateCommandHandler(IApplicationDbContext context)
        : ICommandHandler<CreateWorkoutTemplateCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateWorkoutTemplateCommand command, CancellationToken cancellationToken)
        {

            if (await context.WorkoutTemplates.AnyAsync(t => t.Name == command.Name && t.UserId == command.UserId, cancellationToken))
            {
                return Result.Failure<Guid>(TemplateErrors.TemplateNameNotUnique);
            }

            var exercises = command.Exercises.Select(e => new WorkoutExercise
            {
                ExerciseName = e.ExerciseName,
                RecommendedSets = e.RecommendedSets,
                RepRanges = e.RepRanges
            }).ToList();


            var workout = new WorkoutTemplate
            {
                Name = command.Name,
                Description = command.Description,
                UserId = command.UserId,
                Exercises = exercises
            };


            workout.Raise(new WorkoutTemplateCreatedDomainEvent(workout.Id));


            context.WorkoutTemplates.Add(workout);
            await context.SaveChangesAsync(cancellationToken);

            return workout.Id;
        }
    }
}

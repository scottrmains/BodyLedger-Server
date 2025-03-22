using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Templates.Delete;
using Domain.Assignments;
using Domain.Templates;
using Domain.Templates.Fitness;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Templates.Delete;

internal sealed class DeleteTemplateCommandHandler(IApplicationDbContext context)
        : ICommandHandler<DeleteTemplateCommand>
    {
        public async Task<Result> Handle(DeleteTemplateCommand command, CancellationToken cancellationToken)
        {

        // Check if template exists
        var template = await context.Templates
            .FirstOrDefaultAsync(t => t.Id == command.TemplateId, cancellationToken);

        if (template is null)
        {
            return Result.Failure(TemplateErrors.TemplateNotFound(command.TemplateId));
        }


        // Delete related entities based on template type
        if (template is WorkoutTemplate workoutTemplate)
        {
            // Delete related exercises
            var exercises = await context.WorkoutActivities
                .Where(e => e.WorkoutTemplateId == template.Id)
                .ToListAsync(cancellationToken);

            context.WorkoutActivities.RemoveRange(exercises);
        }

        if (template is FitnessTemplate fitnessTemplate)
        {
            // Delete related exercises
            var activities = await context.FitnessActivities
                .Where(e => e.FitnessTemplateId == template.Id)
                .ToListAsync(cancellationToken);

            context.FitnessActivities.RemoveRange(activities);
        }

        // Remove the template
        context.Templates.Remove(template);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    }



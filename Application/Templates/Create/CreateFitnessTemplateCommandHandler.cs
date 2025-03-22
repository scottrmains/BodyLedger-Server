using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain.Templates.Fitness;

namespace Application.Templates.Create
{
    internal sealed class CreateFitnessTemplateCommandHandler(IApplicationDbContext context)
        : ICommandHandler<CreateFitnessTemplateCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateFitnessTemplateCommand command, CancellationToken cancellationToken)
        {
            if (await context.Templates.AnyAsync(t => t.Name == command.Name && t.UserId == command.UserId, cancellationToken))
            {
                return Result.Failure<Guid>(TemplateErrors.TemplateNameNotUnique);
            }

            var activities = command.Activities.Select(e => new FitnessActivity
            {
                ActivityName = e.ActivityName,
                RecommendedDuration = e.RecommendedDuration,
                IntensityLevel = e.IntensityLevel
            }).ToList();

            var fitnessTemplate = new FitnessTemplate
            {
                Name = command.Name,
                Description = command.Description,
                UserId = command.UserId,
                Activities = activities
            };

            fitnessTemplate.Raise(new TemplateCreatedDomainEvent(fitnessTemplate.Id, TemplateType.Fitness));

            context.Templates.Add(fitnessTemplate);
            await context.SaveChangesAsync(cancellationToken);

            return fitnessTemplate.Id;
        }
    }
}
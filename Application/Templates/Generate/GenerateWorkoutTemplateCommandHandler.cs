using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Templates.GetById;
using Domain.Templates;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Templates.Generate
{
    internal sealed class GenerateWorkoutTemplateCommandHandler(
     IAiService aiService,
     IApplicationDbContext context)
     : ICommandHandler<GenerateWorkoutTemplateCommand, List<WorkoutActivityResponse>>
    {
        public async Task<Result<List<WorkoutActivityResponse>>> Handle(GenerateWorkoutTemplateCommand command, CancellationToken cancellationToken)
        {
            var prompt = $"Create a workout routine called '{command.Name}' with focus on {command.Name}. " +
                         "Return a JSON array with exercises that includes name, sets, repRange for each exercise.";

            var exercises = await aiService.GenerateContent<List<WorkoutActivityResponse>>(
                prompt,
                temperature: 0.5,  // More focused generation
                cancellationToken: cancellationToken
            );

            if (exercises == null || !exercises.Any())
            {
             
            }

            return Result.Success(exercises);
        }
    }
}

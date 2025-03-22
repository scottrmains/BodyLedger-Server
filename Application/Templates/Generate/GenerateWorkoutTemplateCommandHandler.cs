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
     IDeepseekService deepseekService,
     IApplicationDbContext context)
     : ICommandHandler<GenerateWorkoutTemplateCommand, WorkoutTemplateResponse>
    {
        public async Task<Result<WorkoutTemplateResponse>> Handle(
            GenerateWorkoutTemplateCommand command,
            CancellationToken cancellationToken)
        {
            // Call AI service to generate workout exercises based on name and description
            var generatedExercises = await deepseekService.GenerateWorkoutExercises(
                command.Name,
                command.Description,
                cancellationToken);

            if (generatedExercises == null || !generatedExercises.Any())
            {
                return Result.Failure<WorkoutTemplateResponse>(
                    Error.Failure(
                        "AI.WorkoutGeneration",
                        "Failed to generate exercises for this workout"));
            }

            // Create a new workout template from the AI response
            var workoutTemplate = new WorkoutTemplate
            {
                Name = command.Name,
                Description = command.Description,
                UserId = command.UserId,
                Activities = generatedExercises.Select(e => new WorkoutActivity
                {
                    ActivityName = e.Name,
                    RecommendedSets = e.Sets,
                    RepRanges = e.RepRange
                }).ToList()
            };



            // Map to response and return
            var response = new WorkoutTemplateResponse
            {
                Id = workoutTemplate.Id,
                UserId = workoutTemplate.UserId,
                Name = workoutTemplate.Name,
                Description = workoutTemplate.Description,
                TemplateType = TemplateType.Workout.ToString(),
                Activities = workoutTemplate.Activities.Select(e => new WorkoutActivityResponse
                {
                    ActivityName = e.ActivityName,
                    RecommendedSets = e.RecommendedSets,
                    RepRanges = e.RepRanges
                }).ToList()
            };

            return Result.Success(response);
        }
    }
}

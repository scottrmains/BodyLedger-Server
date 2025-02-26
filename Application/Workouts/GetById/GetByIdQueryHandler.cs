using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain.Templates;
using Application.Abstractions.Authentication;
using Domain.Users;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Workouts.GetById;

internal sealed class GetWorkoutTemplateByIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetWorkoutTemplateByIdQuery, WorkoutTemplateResponse>
{
    public async Task<Result<WorkoutTemplateResponse>> Handle(GetWorkoutTemplateByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<WorkoutTemplateResponse>(UserErrors.Unauthorized());
        }
  
        var template = await context.WorkoutTemplates
            .Where(tc => tc.UserId == query.UserId && tc.Id == query.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (template is null)
        {
            return Result.Failure<WorkoutTemplateResponse>(TemplateErrors.TemplateNotFound(query.Id));
        }

        var response = new WorkoutTemplateResponse
        {

            Id = template.Id,
            UserId = template.UserId,
            Name = template.Name,
            Description = template.Description,
            Exercises = template.Exercises.Select(e => new WorkoutExerciseResponse
            {
                ExerciseName = e.ExerciseName,
                RecommendedSets = e.RecommendedSets,
                RepRanges = e.RepRanges
            }).ToList()
        };
   
        return response;
    }
}

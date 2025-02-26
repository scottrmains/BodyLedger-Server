using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain.Templates;
using Application.Abstractions.Authentication;
using Domain.Users;
using Microsoft.Extensions.Caching.Memory;
using Application.Workouts.GetById;

namespace Application.Workouts.GetAllByUserId;

internal sealed class GetWorkoutTemplatesByUserIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetWorkoutTemplatesByUserIdQuery, WorkoutTemplateListResponse>
{
    public async Task<Result<WorkoutTemplateListResponse>> Handle(GetWorkoutTemplatesByUserIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<WorkoutTemplateListResponse>(UserErrors.Unauthorized());
        }
  
        int totalTemplates = await context.WorkoutTemplates
            .Where(tc => tc.UserId == query.UserId)
            .CountAsync(cancellationToken);

        var workoutTemplates = await context.WorkoutTemplates
            .Where(tc => tc.UserId == query.UserId)
            .Include(x => x.Exercises)
            .OrderByDescending(tc => tc.Id) 
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var response = new WorkoutTemplateListResponse
        {
            Items = workoutTemplates.Select(tc => new WorkoutTemplateResponse
            {
                Id = tc.Id,
                UserId = tc.UserId,
                Name = tc.Name,
                Description = tc.Description,
                Exercises = tc.Exercises.Select(e => new WorkoutExerciseResponse
                {
                    ExerciseName = e.ExerciseName,
                    RecommendedSets = e.RecommendedSets,
                    RepRanges = e.RepRanges
                }).ToList()
            }).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalTemplates
        };



        return response;
    }
}

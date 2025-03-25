using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain.Templates;
using Application.Abstractions.Authentication;
using Domain.Users;
using Application.Assignments.Mapping;
using SharedKernel.Responses;

namespace Application.Assignments.GetById;

internal sealed class GetAssignmentByIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext, AssignmentMapper mapper)
    : IQueryHandler<GetAssignmentByIdQuery, AssignmentResponse>
{
    public async Task<Result<AssignmentResponse>> Handle(GetAssignmentByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<AssignmentResponse>(UserErrors.Unauthorized());
        }

        // Query assignment with related data
        var assignment = await context.Assignments
            .Where(a => a.Id == query.Id)
            .Include(a => a.Template)
            .Include(a => a.Items)
            .FirstOrDefaultAsync(cancellationToken);

        if (assignment is null)
        {
            return Result.Failure<AssignmentResponse>(TemplateErrors.TemplateNotFound(query.Id));
        }

        // Check if this user has access to this assignment
        if (assignment.Template.UserId != query.UserId)
        {
            return Result.Failure<AssignmentResponse>(UserErrors.Unauthorized());
        }

        // Get assignment item IDs for further loading
        var assignmentItemIds = assignment.Items.Select(i => i.Id).ToList();

        // Load specific assignment item types
        var workoutActivityAssignments = await context.WorkoutActivityAssignments
            .Where(w => assignmentItemIds.Contains(w.Id))
            .Include(w => w.WorkoutActivity)
            .ToListAsync(cancellationToken);

        var fitnessActivityAssignments = await context.FitnessActivityAssignments
         .Where(w => assignmentItemIds.Contains(w.Id))
         .Include(w => w.FitnessExercise)
         .ToListAsync(cancellationToken);

        // Use the mapper to create the response
        return Result.Success(mapper.MapAssignmentToResponse(assignment, workoutActivityAssignments, fitnessActivityAssignments));

    }


}

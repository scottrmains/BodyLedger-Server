
using Application.AssignmentItems.Complete;
using MediatR;
using SharedKernel;
using SharedKernel.Responses;

namespace Web.Api.Requests.AssignmentItems
{
    public class CompleteWorkoutItemRequest : CompleteItemRequest
    {
        public List<WorkoutSetDto> Sets { get; set; } = new();

        public override IRequest<Result> CreateCommand(Guid itemId, Guid userId)
        {
            return new CompleteWorkoutItemCommand(
                itemId,
                AssignmentId,
                userId,
                Sets);
        }
    }
}


using Application.AssignmentItems.Complete;
using MediatR;
using SharedKernel;

namespace Web.Api.Requests.AssignmentItems
{
    public class CompleteWorkoutItemRequest : CompleteItemRequest
    {
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int? Weight { get; set; }

        public override IRequest<Result> CreateCommand(Guid itemId, Guid userId)
        {
            return new CompleteWorkoutItemCommand(
                itemId,
                AssignmentId,
                userId,
                Sets,
                Reps,
                Weight);
        }
    }
}

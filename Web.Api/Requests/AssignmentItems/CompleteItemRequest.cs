using MediatR;
using SharedKernel;
using System.Text.Json.Serialization;

namespace Web.Api.Requests.AssignmentItems
{

    public abstract class CompleteItemRequest
    {
        public Guid AssignmentId { get; set; }

        public abstract IRequest<Result> CreateCommand(Guid itemId, Guid userId);
    }
}

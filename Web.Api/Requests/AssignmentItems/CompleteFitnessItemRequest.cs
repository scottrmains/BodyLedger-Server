using Application.AssignmentItems.Complete;
using Application.AssignmentItems.CompleteById;
using MediatR;
using SharedKernel;
using System;

namespace Web.Api.Requests.AssignmentItems
{
    public class CompleteFitnessItemRequest : CompleteItemRequest
    {
        public int Duration { get; set; }
        public string Intensity { get; set; }

        public override IRequest<Result> CreateCommand(Guid itemId, Guid userId)
        {
            return new CompleteFitnessItemCommand(
                itemId,
                AssignmentId,
                userId,
                Duration,
                Intensity);
        }
    }
}
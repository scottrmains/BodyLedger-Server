using Application.Abstractions.Messaging;

namespace Application.AssignmentItems.CompleteById
{
    public sealed record CompleteFitnessItemCommand(
        Guid ItemId,
        Guid AssignmentId,
        Guid UserId,
        int Duration,
        string Intensity) : ICommand;

}

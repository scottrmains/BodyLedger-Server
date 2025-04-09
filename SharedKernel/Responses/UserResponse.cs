namespace SharedKernel.Responses;

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }
    public DateTime DateCreated { get; init; }

    public string Role { get; init; }
}

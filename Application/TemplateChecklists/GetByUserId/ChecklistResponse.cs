
using Domain.Templates;
using SharedKernel;
using System.Text.Json.Serialization;

namespace Application.TemplateChecklists.GetByUserId;


public sealed class ChecklistResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public DateTime StartDate { get; init; }
    public DayOfWeek StartDay { get; set; }
    public double CompletionPercentage { get; init; }
    public bool IsComplete { get; init; }
    public bool IsCurrent {get; init; }
    public List<AssignmentResponse> Assignments { get; init; } = new();
}

public sealed class AssignmentResponse
{
    public Guid Id { get; init; }
    public Guid TemplateId { get; init; }
    public string TemplateName { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TemplateType Type { get; init; }
    public string ScheduledDay { get; init; }
    public bool Completed { get; init; }
    public List<AssignmentItemResponse> Items { get; init; } = new();
}

public sealed class AssignmentItemResponse
{
    public Guid Id { get; init; }
    public bool Completed { get; init; }
}

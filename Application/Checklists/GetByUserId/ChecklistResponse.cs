using Application.Assignments.GetById;
using Domain.Assignments;
using Domain.Templates;
using System.Text.Json.Serialization;

namespace Application.Checklists.GetByUserId;

public sealed class ChecklistsResponse
{
    public ChecklistResponse CurrentChecklist { get; set; }
    public ChecklistResponse PreviousChecklist { get; set; }
    public ChecklistResponse FutureChecklist { get; set; }
    public List<DateRangeInfo> DateRanges { get; set; } = new();
    public CalendarBounds CalendarBounds { get; set; }
}

public sealed class ChecklistResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public DateTime StartDate { get; init; }
    public DayOfWeek StartDay { get; set; }
    public double CompletionPercentage { get; init; }
    public bool IsComplete { get; init; }
    public bool IsCurrent { get; init; }
    public List<AssignmentResponse> Assignments { get; init; } = new();
}

public  class AssignmentResponse
{
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public string TemplateName { get; set; }
    public string ScheduledDay { get; set; }
    public string TimeOfDay { get; set; }
    public bool Completed { get; set; }
    public DateTime? CompletedDate { get; set; }
    public Guid ChecklistId { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime RecurringStartDate { get; set; }
    public int ItemsCount { get; set; }
    public int CompletedItemsCount { get; set; }

    // Type discriminator for clients
    public virtual string Type => "Assignment";
}

public sealed class AssignmentItemResponse
{
    public Guid Id { get; init; }
    public bool Completed { get; init; }
}

public sealed class DateRangeInfo
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public bool IsCurrent { get; init; }
    public bool HasData { get; init; }
}

public sealed class CalendarBounds
{
    public DateTime MinDate { get; init; }
    public DateTime MaxDate { get; init; }
}
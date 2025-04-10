using SharedKernel;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Checklists;


public class ChecklistLog : Entity
{
    public Guid ChecklistId { get; set; }
    public Checklist Checklist { get; set; }
    public DateTime Date { get; set; }
    public double? Weight { get; set; }
    public string? Notes { get; set; }
    public MoodType? Mood { get; set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;

    // Private constructor for EF Core
    private ChecklistLog() { }

    public ChecklistLog(Guid checklistId, DateTime date)
    {
        ChecklistId = checklistId;
        Date = date;
        Mood = MoodType.Neutral;
    }

    public void Update(double? weight, string notes, MoodType mood)
    {
        Weight = weight;
        Notes = notes;
        Mood = mood;
        UpdatedAt = DateTime.UtcNow;
    }
}

using Domain.Assignments;
using SharedKernel;

namespace Domain.Checklists;

    public class Checklist : Entity
    {
        public Guid UserId { get; set; }
        public DayOfWeek StartDay { get; set; }
        public DateTime StartDate { get; set; } 
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

        public DateTime DateCreated { get; protected set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; protected set; } = DateTime.UtcNow;

        public bool IsComplete => Assignments.Any() && Assignments.All(a => a.Completed);

        public double CompletionPercentage => Assignments.Any()
            ? (double)Assignments.Count(a => a.Completed) / Assignments.Count * 100
            : 0;

        public DateTime GetCycleStartDate(DateTime referenceDate)
        {
            var startDate = referenceDate.Date;
            // Decrement until we hit the checklist's StartDay (e.g. Monday)
            while (startDate.DayOfWeek != StartDay)
            {
                startDate = startDate.AddDays(-1);
            }
            return startDate;
        }

    }


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Responses
{

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
        public List<DateRangeInfo>? DateRanges { get; set; } = new();
        public CalendarBounds? CalendarBounds { get; set; }
    }

}

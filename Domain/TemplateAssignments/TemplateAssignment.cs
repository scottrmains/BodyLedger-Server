using Domain.Checklist;
using Domain.Templates;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Assignments
{
    public class TemplateAssignment : Entity
    {
        public Guid TemplateId { get; set; }
        public Template Template { get; set; }

        public DayOfWeek ScheduledDay { get; set; }

        public TimeOfDay TimeOfDay {get;set;}

        public bool Completed { get; private set; }
        public DateTime? CompletedDate { get; private set; }

        public Guid TemplateChecklistId { get; set; }
        public WeeklyChecklist TemplateChecklist { get; set; }


        public bool IsRecurring { get; set; } = false;

        public ICollection<AssignmentItem> Items { get; set; } = new List<AssignmentItem>();

        public void MarkItemCompleted(Guid itemId)
        {
            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                item.MarkCompleted();
                if (Items.All(i => i.Completed))
                {
                    MarkCompleted();
                }
            }
        }

        public void MarkCompleted()
        {
            Completed = true;
            CompletedDate = DateTime.UtcNow;
            Raise(new TemplateAssignmentCompletedDomainEvent(Id));
        }
    }

    public enum TimeOfDay
    {
        Anytime = 1,
        Morning = 2,
        Afternoon = 3,
        Evening = 4
    }

    public class AssignmentItem : Entity
    {
        public bool Completed { get; private set; }
        public void MarkCompleted() => Completed = true;
    }


}

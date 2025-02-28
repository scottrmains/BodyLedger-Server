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
        public DateTime RecurringStartDate { get; set; }

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

        public void UndoItemCompletion(Guid itemId)
        {
            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                item.UndoCompletion();
                if (Completed)
                {
                    UndoCompletion();
                }
            }
        }

        public void MarkCompleted()
        {
            Completed = true;
            CompletedDate = DateTime.UtcNow;
            Raise(new TemplateAssignmentCompletedDomainEvent(Id));
        }

        public void UndoCompletion()
        {
            Completed = false;
            CompletedDate = null;

             foreach (var item in Items)
            {
               item.UndoCompletion();
            }

            Raise(new TemplateAssignmentUndoDomainEvent(Id));
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
        public DateTime? CompletedDate { get; private set; }
        public void MarkCompleted()
        {
            Completed = true;
            CompletedDate = DateTime.UtcNow;
        }

        public void UndoCompletion()
        {
            Completed = false;
            CompletedDate = null;
        }
    }


}

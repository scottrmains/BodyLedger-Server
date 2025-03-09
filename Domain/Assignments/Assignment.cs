
using Domain.Templates;
using SharedKernel;
using Domain.Checklists;

namespace Domain.Assignments
{
    public class Assignment : Entity
    {
        public Guid TemplateId { get; set; }
        public Template Template { get; set; }

        public DayOfWeek ScheduledDay { get; set; }

        public TimeOfDay TimeOfDay {get;set;}

        public bool Completed { get; private set; }
        public DateTime? CompletedDate { get; private set; }

        public Guid ChecklistId { get; set; }
        public Checklist Checklist { get; set; }


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

        public  void MarkCompleted()
        {
            Completed = true;
            CompletedDate = DateTime.UtcNow;
            Raise(new AssignmentCompletedDomainEvent(Id));
        }

        public  void UndoCompletion()
        {
            Completed = false;
            CompletedDate = null;

             foreach (var item in Items)
            {
               item.UndoCompletion();
            }

            Raise(new AssignmentUndoDomainEvent(Id));
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
        public Guid TemplateAssignmentId { get; set; }
        public bool Completed { get; private set; }
        public DateTime? CompletedDate { get; private set; }

        public Assignment TemplateAssignment { get; set; }
        public virtual void MarkCompleted() 
        {
            Completed = true;
            CompletedDate = DateTime.UtcNow;
        }

        public virtual void UndoCompletion()
        {
            Completed = false;
            CompletedDate = null;
        }
    }
}




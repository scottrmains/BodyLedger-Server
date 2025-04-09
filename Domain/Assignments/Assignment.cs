
using Domain.Templates;
using SharedKernel;
using Domain.Checklists;
using SharedKernel.Enums;

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

        public void CheckCompletion()
        {
            if (!Completed && Items.All(i => i.Completed))
            {
                MarkCompleted();
            }
        }


        public void MarkCompleted()
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




}




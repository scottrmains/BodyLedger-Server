using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Assignments
{
    public class AssignmentItem : Entity
    {
        public Guid AssignmentId { get; set; }
        public bool Completed { get; protected set; }
        public DateTime? CompletedDate { get; protected set; }
        public Assignment Assignment { get; set; }

        public virtual void MarkCompleted()
        {
            Completed = true;
            CompletedDate = DateTime.UtcNow;
            Assignment?.CheckCompletion();
        }

        public virtual void UndoCompletion()
        {
            Completed = false;
            CompletedDate = null;
            if (Assignment?.Completed == true)
            {
                Assignment.UndoCompletion();
            }
        }
    }
}

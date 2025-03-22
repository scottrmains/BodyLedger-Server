using Domain.Assignments;
using Domain.Templates;


namespace Domain.TemplateAssignments
{
 
    public class WorkoutActivityAssignment : AssignmentItem
    {
        public Guid WorkoutActivityId { get; set; }
        public WorkoutActivity WorkoutActivity { get; set; }
        public int? CompletedSets { get; set; }
        public int? CompletedReps { get; set; }
        public int? ActualWeight { get; set; }

        public void MarkCompleted(int? sets = null, int reps = 0, int? weight = null)
        {
            CompletedSets = sets;
            CompletedReps = reps;
            ActualWeight = weight;
            base.MarkCompleted(); 
        }

        public new void UndoCompletion()
        {
            base.UndoCompletion(); 
            CompletedSets = null;
            CompletedReps = 0;
            ActualWeight = null;
        }
    }
}
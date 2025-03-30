using Domain.Assignments;
using Domain.Templates;


namespace Domain.Assignments
{

    public class WorkoutActivityAssignment : AssignmentItem
    {
        public Guid WorkoutActivityId { get; set; }
        public WorkoutActivity WorkoutActivity { get; set; }
        public ICollection<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();

        public void AddSet(int setNumber, int reps, int? weight)
        {
            var set = new WorkoutSet(Id, setNumber, reps, weight);
            Sets.Add(set);
        }


        public new void UndoCompletion()
        {
            base.UndoCompletion();
            Sets.Clear();
        }
    }
}
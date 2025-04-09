using Domain.Assignments;
using Domain.Templates;


namespace Domain.Assignments
{

    public class WorkoutActivityAssignment : AssignmentItem
    {
        public Guid WorkoutActivityId { get; set; }
        public WorkoutActivity WorkoutActivity { get; set; }
        public ICollection<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();
    }
}
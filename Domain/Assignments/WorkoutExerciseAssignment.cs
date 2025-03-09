using Domain.Assignments;
using Domain.Workouts;
using SharedKernel;
using System;

namespace Domain.TemplateAssignments
{
 
    public class WorkoutExerciseAssignment : AssignmentItem
    {
        public Guid WorkoutExerciseId { get; set; }
        public WorkoutExercise WorkoutExercise { get; set; }
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
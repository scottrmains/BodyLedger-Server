using Domain.Assignments;
using Domain.Workouts;
using SharedKernel;
using System;

namespace Domain.TemplateAssignments
{
    // This is a joining entity that links a TemplateAssignment to specific WorkoutExercises
    public class WorkoutExerciseAssignment : Entity
    {
        public Guid TemplateAssignmentId { get; set; }
        public TemplateAssignment TemplateAssignment { get; set; }

        public Guid WorkoutExerciseId { get; set; }
        public WorkoutExercise WorkoutExercise { get; set; }


        // Tracking progress
        public int? CompletedSets { get; set; }
        public int? CompletedReps { get; set; }
        public int? ActualWeight { get; set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedDate { get; private set; }

        public void MarkCompleted(int? sets = null, int reps = 0, int? weight = null)
        {
            CompletedSets = sets;
            CompletedReps = reps;
            ActualWeight = weight;
            IsCompleted = true;
            CompletedDate = DateTime.UtcNow;
        }

        public void UndoCompletion()
        {
            IsCompleted = false;
            CompletedDate = null;
            CompletedSets = null;
            CompletedReps = 0;
            ActualWeight = null;
        }
    }
}
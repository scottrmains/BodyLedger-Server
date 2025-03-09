using Application.Checklists.GetByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Assignments.GetById
{
    public class WorkoutAssignmentResponse : AssignmentResponse
    {
        public List<WorkoutExerciseAssignmentResponse> ExerciseItems { get; set; } = new();

        public override string Type => "Workout";
    }

    public class WorkoutExerciseAssignmentResponse 
    {
        public Guid Id { get; set; }
        public string ExerciseName { get; set; }
        public int RecommendedSets { get; set; }
        public string RepRanges { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? CompletedSets { get; set; }
        public int? CompletedReps { get; set; }
        public int? ActualWeight { get; set; }

    }
}

using Domain.Templates.Fitness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Assignments
{
    public class FitnessActivityAssignment : AssignmentItem
    {
        public Guid FitnessActivityId { get; set; }
        public FitnessActivity FitnessExercise { get; set; }
        public int? CompletedDuration { get; set; }  
        public string? ActualIntensity { get; set; }  

        public decimal? ActualPace { get; set; }

        public void MarkCompleted(int? duration = null, string intensity = null)
        {
            CompletedDuration = duration;
            ActualIntensity = intensity;
            base.MarkCompleted();
        }

        public new void UndoCompletion()
        {
            base.UndoCompletion();
            CompletedDuration = null;
            ActualIntensity = null;
        }
    }
}

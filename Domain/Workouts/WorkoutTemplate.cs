using Domain.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Workouts
{
    public class WorkoutTemplate : Template
    {
        public WorkoutTemplate()
        {
            Type = TemplateType.Workout;
            Exercises = new List<WorkoutExercise>();
        }

        public ICollection<WorkoutExercise> Exercises { get; set; }

    }
}

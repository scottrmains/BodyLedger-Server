using Domain.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Templates
{
    public class WorkoutTemplate : Template
    {
        public WorkoutTemplate()
        {
            Type = TemplateType.Workout;
            Activities = new List<WorkoutActivity>();
        }

        public ICollection<WorkoutActivity> Activities { get; set; }

    }
}

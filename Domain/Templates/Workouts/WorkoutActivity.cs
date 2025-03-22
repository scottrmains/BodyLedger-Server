using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Templates
{
    public class WorkoutActivity : Entity
    {
        public string ActivityName { get; set; }
        public int RecommendedSets { get; set; }
        public string RepRanges { get; set; } 
        public Guid WorkoutTemplateId { get; set; }
        public WorkoutTemplate WorkoutTemplate { get; set; }
        public DateTime DateCreated { get; protected set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; protected set; } = DateTime.UtcNow;

    }

}

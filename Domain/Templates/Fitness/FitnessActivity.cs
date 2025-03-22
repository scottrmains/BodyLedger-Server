using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Templates.Fitness
{
    public class FitnessActivity : Entity
    {
        public string ActivityName { get; set; }
        public int RecommendedDuration { get; set; }  
        public string? IntensityLevel { get; set; }   
        public Guid FitnessTemplateId { get; set; }
        public FitnessTemplate FitnessTemplate { get; set; }
        public DateTime DateCreated { get; protected set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; protected set; } = DateTime.UtcNow;
    }
}

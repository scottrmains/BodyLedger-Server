using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Templates.Fitness
{
    public class FitnessTemplate : Template
    {
        public FitnessTemplate()
        {
            Type = TemplateType.Fitness;
            Activities = new List<FitnessActivity>();
        }

        public ICollection<FitnessActivity> Activities { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Responses
{
    public class WorkoutSetDto
    {
        public int SetNumber { get; set; }
        public int Reps { get; set; }
        public int? Weight { get; set; }
    }


}

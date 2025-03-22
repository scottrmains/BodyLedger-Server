using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IDeepseekService
    {
        Task<List<GeneratedExercise>> GenerateWorkoutExercises(
            string workoutName,
            string description,
            CancellationToken cancellationToken);
    }


    public class GeneratedExercise
    {
        public string Name { get; set; }
        public int Sets { get; set; }
        public string RepRange { get; set; }
    }
}

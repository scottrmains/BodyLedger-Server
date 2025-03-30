
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Assignments;

        public class WorkoutSet : Entity
        {
            public Guid WorkoutActivityAssignmentId { get; set; }
            public WorkoutActivityAssignment WorkoutActivityAssignment { get; set; }
            public int SetNumber { get; set; }
            public int Reps { get; set; }
            public int? Weight { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            // Private constructor for EF Core
            private WorkoutSet() { }

            public WorkoutSet(Guid workoutActivityAssignmentId, int setNumber, int reps, int? weight)
            {
                WorkoutActivityAssignmentId = workoutActivityAssignmentId;
                SetNumber = setNumber;
                Reps = reps;
                Weight = weight;
            }
        }



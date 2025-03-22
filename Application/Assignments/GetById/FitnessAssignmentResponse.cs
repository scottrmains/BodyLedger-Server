using Application.Checklists.GetByUserId;
using System;
using System.Collections.Generic;

namespace Application.Assignments.GetById
{
    public class FitnessAssignmentResponse : AssignmentResponse
    {
        public List<FitnessActivityAssignmentResponse> ActivityItems { get; set; } = new();

        public override string Type => "Fitness";
    }

    public class FitnessActivityAssignmentResponse
    {
        public Guid Id { get; set; }
        public string ActivityName { get; set; }
        public int RecommendedDuration { get; set; }
        public string IntensityLevel { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? CompletedDuration { get; set; }
        public string ActualIntensity { get; set; }
    }
}
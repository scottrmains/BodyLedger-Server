
namespace SharedKernel.Responses;

    public sealed class FitnessAssignmentResponse : AssignmentResponse
    {
        public List<FitnessActivityAssignmentResponse> ActivityItems { get; init; } = new();

        public override TemplateType Type => TemplateType.Fitness;
    }

    public sealed class FitnessActivityAssignmentResponse
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

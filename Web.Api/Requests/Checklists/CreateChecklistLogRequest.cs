using SharedKernel.Enums;

namespace Web.Api.Requests.Checklists
{
    public class CreateChecklistLogRequest
    {
        public DateTime Date { get; set; }
        public double? Weight { get; set; }
        public string Notes { get; set; }
        public MoodType Mood { get; set; } = MoodType.Neutral;
    }
}

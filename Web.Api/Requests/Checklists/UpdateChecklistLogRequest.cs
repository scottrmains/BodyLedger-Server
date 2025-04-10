using SharedKernel.Enums;

namespace Web.Api.Requests.Checklists
{
    public class UpdateChecklistLogRequest
    {
        public double? Weight { get; set; }
        public string Notes { get; set; }
        public MoodType Mood { get; set; }
    }
}

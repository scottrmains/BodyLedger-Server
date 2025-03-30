namespace Web.Api.Requests.AssignmentItems
{
    public class WorkoutSetRequest
    {
        public int SetNumber { get; set; }
        public int Reps { get; set; }
        public int? Weight { get; set; }
    }
}

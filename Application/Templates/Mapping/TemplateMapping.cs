
using Application.Templates.GetById;
using Domain.Templates;
using Domain.Templates.Fitness;


namespace Application.Templates.Mapping
    {
        public class TemplateMapper
        {
            public TemplateResponse MapTemplateToResponse(Template template, Dictionary<Guid, List<WorkoutActivity>> workoutActivities = null, Dictionary<Guid, List<FitnessActivity>> fitnessActivities = null)
            {
                return template switch
                {
                    WorkoutTemplate workout => MapWorkoutTemplate(workout, workoutActivities),
                    FitnessTemplate fitness => MapFitnessTemplate(fitness, fitnessActivities),
                    _ => MapBaseTemplate(template)
                };
            }

            public WorkoutTemplateResponse MapWorkoutTemplate(
                WorkoutTemplate workout,
                Dictionary<Guid, List<WorkoutActivity>> workoutActivities)
            {
                return new WorkoutTemplateResponse
                {
                    Id = workout.Id,
                    UserId = workout.UserId,
                    Name = workout.Name,
                    Description = workout.Description,
                    TemplateType = TemplateType.Workout.ToString(),
                    Activities = workoutActivities != null && workoutActivities.TryGetValue(workout.Id, out var activities)
                        ? activities.Select(MapWorkoutActivity).ToList()
                        : workout.Activities?.Select(MapWorkoutActivity).ToList() ?? new List<WorkoutActivityResponse>()
                };
            }


        public FitnessTemplateResponse MapFitnessTemplate(
              FitnessTemplate fitness,
              Dictionary<Guid, List<FitnessActivity>> fitnessActivities)
                {
                    return new FitnessTemplateResponse
                    {
                        Id = fitness.Id,
                        UserId = fitness.UserId,
                        Name = fitness.Name,
                        Description = fitness.Description,
                        TemplateType = TemplateType.Fitness.ToString(),
                        Activities = fitnessActivities != null && fitnessActivities.TryGetValue(fitness.Id, out var activities)
                            ? activities.Select(MapFitnessActivity).ToList()
                            : fitness.Activities?.Select(MapFitnessActivity).ToList() ?? new List<FitnessActivityResponse>()
                    };
                }

        public WorkoutActivityResponse MapWorkoutActivity(WorkoutActivity activity)
            {
                return new WorkoutActivityResponse
                {
                    ActivityName = activity.ActivityName,
                    RecommendedSets = activity.RecommendedSets,
                    RepRanges = activity.RepRanges
                };
            }


        public FitnessActivityResponse MapFitnessActivity(FitnessActivity activity)
        {
            return new FitnessActivityResponse
            {
                ActivityName = activity.ActivityName,
                IntensityLevel = activity.IntensityLevel,
                RecommendedDuration = activity.RecommendedDuration
            };
        }
        public TemplateResponse MapBaseTemplate(Template template)
            {
                return new TemplateResponse
                {
                    Id = template.Id,
                    UserId = template.UserId,
                    Name = template.Name,
                    Description = template.Description,
                };
            }
        }
    }


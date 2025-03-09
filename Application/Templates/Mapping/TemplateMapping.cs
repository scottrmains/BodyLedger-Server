
using Application.Templates.GetById;
using Domain.Templates;
using Domain.Workouts;

namespace Application.Templates.Mapping
    {
        public class TemplateMapper
        {
            public TemplateResponse MapTemplateToResponse(Template template, Dictionary<Guid, List<WorkoutExercise>> workoutExercises = null)
            {
                return template switch
                {
                    WorkoutTemplate workout => MapWorkoutTemplate(workout, workoutExercises),

                    _ => MapBaseTemplate(template)
                };
            }

            public WorkoutTemplateResponse MapWorkoutTemplate(
                WorkoutTemplate workout,
                Dictionary<Guid, List<WorkoutExercise>> workoutExercises)
            {
                return new WorkoutTemplateResponse
                {
                    Id = workout.Id,
                    UserId = workout.UserId,
                    Name = workout.Name,
                    Description = workout.Description,
                    TemplateType = TemplateType.Workout,
                    Exercises = workoutExercises != null && workoutExercises.TryGetValue(workout.Id, out var exercises)
                        ? exercises.Select(MapWorkoutExercise).ToList()
                        : workout.Exercises?.Select(MapWorkoutExercise).ToList() ?? new List<WorkoutExerciseResponse>()
                };
            }

            public WorkoutExerciseResponse MapWorkoutExercise(WorkoutExercise exercise)
            {
                return new WorkoutExerciseResponse
                {
                    ExerciseName = exercise.ExerciseName,
                    RecommendedSets = exercise.RecommendedSets,
                    RepRanges = exercise.RepRanges
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
                    TemplateType = TemplateType.Unknown
                };
            }
        }
    }


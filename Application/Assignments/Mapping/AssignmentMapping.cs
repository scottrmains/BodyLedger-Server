
using Domain.Assignments;
using SharedKernel.Responses;

namespace Application.Assignments.Mapping
{
    public class AssignmentMapper
    {
        public AssignmentResponse MapAssignmentToResponse(
            Assignment assignment,
            List<WorkoutActivityAssignment> workoutActivityAssignments = null,
            List<FitnessActivityAssignment> fitnessActivityAssignments = null)
        {
            // First, create a base assignment response with common properties
            var baseResponse = MapBaseAssignment(assignment);

            // Then, based on template type, create a specialized response
            if (workoutActivityAssignments != null && workoutActivityAssignments.Any())
            {
                return CreateWorkoutResponse(baseResponse, workoutActivityAssignments);
            }
            else if (fitnessActivityAssignments != null && fitnessActivityAssignments.Any())
            {
                return CreateFitnessResponse(baseResponse, fitnessActivityAssignments);
            }

            // If no specific type is found, return the base response
            return baseResponse;
        }

        public AssignmentResponse MapBaseAssignment(Assignment assignment)
        {
            return new AssignmentResponse
            {
                Id = assignment.Id,
                TemplateId = assignment.TemplateId,
                TemplateName = assignment.Template?.Name ?? "Unknown Template",
                ScheduledDay = assignment.ScheduledDay.ToString(),
                TimeOfDay = assignment.TimeOfDay.ToString(),
                Completed = assignment.Completed,
                CompletedDate = assignment.CompletedDate,
                ChecklistId = assignment.ChecklistId,
                IsRecurring = assignment.IsRecurring,
                RecurringStartDate = assignment.RecurringStartDate,
                ItemsCount = assignment.Items?.Count ?? 0,
                CompletedItemsCount = assignment.Items?.Count(i => i.Completed) ?? 0
            };
        }

        private WorkoutAssignmentResponse CreateWorkoutResponse(
            AssignmentResponse baseResponse,
            List<WorkoutActivityAssignment> workoutActivityAssignments)
        {
            var workoutResponse = new WorkoutAssignmentResponse
            {
                Id = baseResponse.Id,
                TemplateId = baseResponse.TemplateId,
                TemplateName = baseResponse.TemplateName,
                ScheduledDay = baseResponse.ScheduledDay,
                TimeOfDay = baseResponse.TimeOfDay,
                Completed = baseResponse.Completed,
                CompletedDate = baseResponse.CompletedDate,
                ChecklistId = baseResponse.ChecklistId,
                IsRecurring = baseResponse.IsRecurring,
                RecurringStartDate = baseResponse.RecurringStartDate,
                ItemsCount = baseResponse.ItemsCount,
                CompletedItemsCount = baseResponse.CompletedItemsCount,
                ActivityItems = workoutActivityAssignments.Select(MapWorkoutExerciseAssignment).ToList()
            };

            return workoutResponse;
        }

        private FitnessAssignmentResponse CreateFitnessResponse(
            AssignmentResponse baseResponse,
            List<FitnessActivityAssignment> fitnessExerciseAssignments)
        {
            var fitnessResponse = new FitnessAssignmentResponse
            {
                Id = baseResponse.Id,
                TemplateId = baseResponse.TemplateId,
                TemplateName = baseResponse.TemplateName,
                ScheduledDay = baseResponse.ScheduledDay,
                TimeOfDay = baseResponse.TimeOfDay,
                Completed = baseResponse.Completed,
                CompletedDate = baseResponse.CompletedDate,
                ChecklistId = baseResponse.ChecklistId,
                IsRecurring = baseResponse.IsRecurring,
                RecurringStartDate = baseResponse.RecurringStartDate,
                ItemsCount = baseResponse.ItemsCount,
                CompletedItemsCount = baseResponse.CompletedItemsCount,
                ActivityItems = fitnessExerciseAssignments.Select(MapFitnessExerciseAssignment).ToList()
            };

            return fitnessResponse;
        }

        // Add this method to the AssignmentMapper class
        public WorkoutSetDto MapWorkoutSet(WorkoutSet set)
        {
            return new WorkoutSetDto
            {
                SetNumber = set.SetNumber,
                Reps = set.Reps,
                Weight = set.Weight
            };
        }

        // Replace the existing MapWorkoutExerciseAssignment method with this one
        public WorkoutActivityAssignmentResponse MapWorkoutExerciseAssignment(WorkoutActivityAssignment assignment)
        {
            var response = new WorkoutActivityAssignmentResponse
            {
                Id = assignment.Id,
                ActivityName = assignment.WorkoutActivity?.ActivityName ?? "Unknown Activity",
                RecommendedSets = assignment.WorkoutActivity?.RecommendedSets ?? 0,
                RepRanges = assignment.WorkoutActivity?.RepRanges ?? "0",
                Completed = assignment.Completed,
                CompletedDate = assignment.CompletedDate
            };

            // Map individual sets if any
            if (assignment.Sets != null && assignment.Sets.Any())
            {
                response.Sets = assignment.Sets
                    .OrderBy(s => s.SetNumber)
                    .Select(MapWorkoutSet)
                    .ToList();
            }

            return response;
        }

        public FitnessActivityAssignmentResponse MapFitnessExerciseAssignment(FitnessActivityAssignment assignment)
        {
            return new FitnessActivityAssignmentResponse
            {
                Id = assignment.Id,
                ActivityName = assignment.FitnessExercise?.ActivityName ?? "Unknown Activity",
                RecommendedDuration = assignment.FitnessExercise?.RecommendedDuration ?? 0,
                IntensityLevel = assignment.FitnessExercise?.IntensityLevel ?? "Unknown",
                Completed = assignment.Completed,
                CompletedDate = assignment.CompletedDate,
                CompletedDuration = assignment.CompletedDuration,
                ActualIntensity = assignment.ActualIntensity
            };
        }

        public List<AssignmentResponse> MapAssignmentsToResponses(
                IEnumerable<Assignment> assignments,
                Dictionary<Guid, List<WorkoutActivityAssignment>> workoutActivitiesByAssignment,
                Dictionary<Guid, List<FitnessActivityAssignment>> fitnessActivitiesByAssignment = null)
        {
            return assignments.Select(a =>
            {
                // Get activity assignments for this specific assignment if they exist
                workoutActivitiesByAssignment.TryGetValue(a.Id, out var workoutActivities);
                fitnessActivitiesByAssignment.TryGetValue(a.Id, out var fitnessActivities);

                // Map using existing method
                return MapAssignmentToResponse(a, workoutActivities ?? new List<WorkoutActivityAssignment>(), fitnessActivities ?? new List<FitnessActivityAssignment>());
            }).ToList();
        }
    }
}
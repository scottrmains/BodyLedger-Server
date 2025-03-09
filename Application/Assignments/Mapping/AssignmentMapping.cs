using Application.Assignments.GetById;
using Application.Checklists.GetByUserId;
using Domain.Assignments;
using Domain.TemplateAssignments;
using Domain.Templates;
using Domain.Workouts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Assignments.Mapping
{
    public class AssignmentMapper
    {
        public AssignmentResponse MapAssignmentToResponse(
            Assignment assignment,
            List<WorkoutExerciseAssignment>? workoutExerciseAssignments = null)
        {
            // First, create a base assignment response with common properties
            var baseResponse = MapBaseAssignment(assignment);

            // Then, if this is a workout assignment, create and return a specialized response
            if (workoutExerciseAssignments != null && workoutExerciseAssignments.Any())
            {
                return CreateWorkoutResponse(baseResponse, workoutExerciseAssignments);
            }

            // For other assignment types, add similar checks here

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
            List<WorkoutExerciseAssignment> workoutExerciseAssignments)
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
                ExerciseItems = workoutExerciseAssignments.Select(MapWorkoutExerciseAssignment).ToList()
            };

            return workoutResponse;
        }

        public WorkoutExerciseAssignmentResponse MapWorkoutExerciseAssignment(WorkoutExerciseAssignment assignment)
        {
            return new WorkoutExerciseAssignmentResponse
            {
                Id = assignment.Id,
                ExerciseName = assignment.WorkoutExercise?.ExerciseName ?? "Unknown Exercise",
                RecommendedSets = assignment.WorkoutExercise?.RecommendedSets ?? 0,
                RepRanges = assignment.WorkoutExercise?.RepRanges ?? "0",
                Completed = assignment.Completed,
                CompletedDate = assignment.CompletedDate,
                CompletedSets = assignment.CompletedSets,
                CompletedReps = assignment.CompletedReps,
                ActualWeight = assignment.ActualWeight
            };
        }

        public List<AssignmentResponse> MapAssignmentsToResponses(
                IEnumerable<Assignment> assignments,
                Dictionary<Guid, List<WorkoutExerciseAssignment>> exerciseAssignmentsByAssignment)
        {
            return assignments.Select(a =>
            {
                // Get exercise assignments for this specific assignment if they exist
                exerciseAssignmentsByAssignment.TryGetValue(a.Id, out var exerciseAssignments);

                // Map using existing method
                return MapAssignmentToResponse(a, exerciseAssignments);
            }).ToList();
        }
    }
}
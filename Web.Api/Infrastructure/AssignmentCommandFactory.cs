using Application.Assignments.Schedule;
using Domain.Templates;
using MediatR;
using SharedKernel;
using System;

namespace Web.Api.Infrastructure
{
    public static class AssignmentCommandFactory
    {
        public static IRequest<Result<Guid>> CreateScheduleCommand(
            TemplateType templateType,
            Guid userId,
            Guid checklistId,
            Guid templateId,
            DayOfWeek scheduledDay,
            bool isRecurring)
        {
            return templateType switch
            {
                TemplateType.Workout => new ScheduleWorkoutAssignmentCommand(
                    userId,
                    checklistId,
                    templateId,
                    scheduledDay,
                    isRecurring),

                TemplateType.Fitness => new ScheduleFitnessAssignmentCommand(
                    userId,
                    checklistId,
                    templateId,
                    scheduledDay,
                    isRecurring),

                _ => throw new ArgumentException($"Unsupported template type: {templateType}")
            };
        }
    }
}
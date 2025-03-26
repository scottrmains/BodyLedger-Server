using Application.Assignments.Schedule;
using Domain.Assignments;
using Domain.Templates;
using MediatR;
using SharedKernel;
using SharedKernel.Enums;
using System;

namespace Web.Api.Infrastructure
{
    public static class AssignmentCommandFactory
    {
        public static IRequest<Result<Guid>> CreateScheduleCommand(
            TemplateType templateType,
            Guid userId,
            DateTime selectedDate,
            Guid templateId,
            DayOfWeek scheduledDay,
            TimeOfDay timeOfDay,
            bool isRecurring)
        {
            return templateType switch
            {
                TemplateType.Workout => new ScheduleWorkoutAssignmentCommand(
                    userId,
                    selectedDate,
                    templateId,
                    scheduledDay,
                    timeOfDay,
                    isRecurring),

                TemplateType.Fitness => new ScheduleFitnessAssignmentCommand(
                    userId,
                    selectedDate,
                    templateId,
                    scheduledDay,
                    timeOfDay,
                    isRecurring),

                _ => throw new ArgumentException($"Unsupported template type: {templateType}")
            };
        }
    }
}
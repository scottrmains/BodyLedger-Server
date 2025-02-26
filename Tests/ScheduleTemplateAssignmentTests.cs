
using Domain.Templates;
using Domain.Workouts;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using SharedKernel;
using Application.Abstractions.Data;
using Tests;
using Application.Abstractions.Services;
using Application.TemplateAssignments.Schedule;
using Domain.Checklist;

namespace Tests
{
    public class ScheduleTemplateAssignmentTests
    {
        // Helper to create a fresh in-memory ApplicationDbContext for each test.
        private IApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options, new FakePublisher());
        }

        private IChecklistService checklistService;

        [Fact]
        public async Task Handle_Should_Create_Assignment_When_Valid()
        {
            // Arrange
            var context = CreateInMemoryContext();
            Guid testUserId = Guid.NewGuid();

            var checklist = new WeeklyChecklist
            {
                UserId = testUserId,
                StartDay = DayOfWeek.Monday
            };
            context.WeeklyChecklists.Add(checklist);

            var workoutTemplate = new WorkoutTemplate
            {
                Name = "Test Workout",
                Description = "Test Description",
                UserId = testUserId,
                Exercises = new List<WorkoutExercise>
        {
            new WorkoutExercise { ExerciseName = "Push Up", RecommendedSets = 3, RepRanges = "10-15" }
        }
            };
            context.WorkoutTemplates.Add(workoutTemplate);
            await context.SaveChangesAsync(CancellationToken.None);
            var scheduledDay = DayOfWeek.Sunday;
            // ✅ The user schedules an assignment for Wednesday, March 6th, 2024
            var scheduledDate = new DateTime(2024, 3, 6);
            var command = new ScheduleTemplateAssignmentCommand(
                ChecklistId: checklist.Id,
                TemplateId: workoutTemplate.Id,
                ScheduledDay: scheduledDay,
                true,
                UserId: testUserId
            );

            var handler = new ScheduleTemplateAssignmentCommandHandler(context, checklistService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue("a valid assignment should be created");
            var assignment = await context.TemplateAssignments
                .FirstOrDefaultAsync(a => a.TemplateChecklistId == checklist.Id, CancellationToken.None);
            assignment.Should().NotBeNull();

            // ✅ The offset should be 2 (Wednesday is 2 days after Monday)
   
            assignment.TemplateId.Should().Be(workoutTemplate.Id);
        }


        [Fact]
        public async Task Handle_Should_Return_Error_When_Assignment_For_Same_Day_Exists()
        {
            // Arrange
            var context = CreateInMemoryContext();
            Guid testUserId = Guid.NewGuid();

            var checklist = new WeeklyChecklist
            {
                UserId = testUserId,
                StartDay = DayOfWeek.Monday,

            };
            context.WeeklyChecklists.Add(checklist);

            var workoutTemplate = new WorkoutTemplate
            {
                Name = "Test Workout",
                Description = "Test Description",
                UserId = testUserId,
                Exercises = new List<WorkoutExercise>()
            };
            context.WorkoutTemplates.Add(workoutTemplate);
            await context.SaveChangesAsync(CancellationToken.None);
            var scheduledDay = DayOfWeek.Sunday;
            // Create an initial assignment for cycle day offset 1 (e.g., Tuesday).
            var initialCommand = new ScheduleTemplateAssignmentCommand(
                ChecklistId: checklist.Id,
                TemplateId: workoutTemplate.Id,
                ScheduledDay: scheduledDay,
                true,
                UserId: testUserId
            );
            var handler = new ScheduleTemplateAssignmentCommandHandler(context, checklistService);
            var initialResult = await handler.Handle(initialCommand, CancellationToken.None);
            initialResult.IsSuccess.Should().BeTrue("the first assignment should be created");

            // Act: Try to schedule another assignment for the same day.
            var duplicateCommand = new ScheduleTemplateAssignmentCommand(
                  ChecklistId: checklist.Id,
                TemplateId: workoutTemplate.Id,
                ScheduledDay: scheduledDay,
                true,
                UserId: testUserId
            );
            var duplicateResult = await handler.Handle(duplicateCommand, CancellationToken.None);

            // Assert
            duplicateResult.IsSuccess.Should().BeFalse("an assignment for the same day should not be allowed");
            duplicateResult.Error.Should().Be(TemplateErrors.TemplateAssignmentExists(checklist.Id, "1"));
        }

        [Fact]
        public async Task Handle_Should_Return_Error_When_Checklist_Not_Found()
        {
            // Arrange
            var context = CreateInMemoryContext();
            Guid testUserId = Guid.NewGuid();

            var workoutTemplate = new WorkoutTemplate
            {
                Name = "Test Workout",
                Description = "Test Description",
                UserId = testUserId,
                Exercises = new List<WorkoutExercise>()
            };
            context.WorkoutTemplates.Add(workoutTemplate);
            await context.SaveChangesAsync(CancellationToken.None);
            var scheduledDay = DayOfWeek.Sunday;
            // Act: Attempt to schedule an assignment without a checklist.
            var command = new ScheduleTemplateAssignmentCommand(
                ChecklistId: new Guid(),
                TemplateId: workoutTemplate.Id,
                ScheduledDay: scheduledDay,
                false,
                UserId: testUserId
            );
            var handler = new ScheduleTemplateAssignmentCommandHandler(context, checklistService);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse("scheduling should fail when the checklist does not exist");
            result.Error.Should().Be(TemplateErrors.TemplateChecklistNotFound(testUserId));
        }
    }
}

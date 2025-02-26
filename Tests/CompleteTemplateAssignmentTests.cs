using System;
using System.Threading;
using System.Threading.Tasks;
using Application.TemplateAssignments.Complete;
using Domain.Templates; // Contains TemplateAssignment, TemplateChecklist, AssignmentItem, etc.
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using SharedKernel;
using Application.Abstractions.Data;
using Domain.Workouts;
using Domain.Assignments;
using Domain.Checklist;

namespace Tests
{

    public class CompleteAssignmentItemCommandHandlerTests
    {
        private IApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options, new FakePublisher());
        }

        [Fact]
        public async Task Handle_Should_Mark_Assignment_As_Completed()
        {
            // Arrange
            var context = CreateInMemoryContext();
            Guid testUserId = Guid.NewGuid();

            // Create a TemplateChecklist for the user.
            var checklist = new WeeklyChecklist
            {
                UserId = testUserId,
                StartDate = DateTime.UtcNow
            };
            context.WeeklyChecklists.Add(checklist);
            var workoutTemplate = new WorkoutTemplate
            {
                Name = "Test Workout",
                Description = "Workout Description",
                UserId = testUserId,
                Exercises = new List<WorkoutExercise>()
            };
            context.WorkoutTemplates.Add(workoutTemplate);


            var assignment = new TemplateAssignment
            {
                TemplateId = workoutTemplate.Id,
                Template = workoutTemplate,

                TemplateChecklistId = checklist.Id,
                TemplateChecklist = checklist
            };
            checklist.Assignments.Add(assignment);
            context.TemplateAssignments.Add(assignment);

            await context.SaveChangesAsync(CancellationToken.None);


            var command = new CompleteAssignmentItemCommand(assignment.Id);
            var handler = new CompleteAssignmentItemCommandHandler(context);
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert: Check that the result indicates success.
            result.IsSuccess.Should().BeTrue("completing the assignment should succeed");

            // Reload the assignment from the database.
            var updatedAssignment = await ((ApplicationDbContext)context).TemplateAssignments
                .FirstOrDefaultAsync(a => a.Id == assignment.Id, cancellationToken);

            updatedAssignment.Should().NotBeNull();
            updatedAssignment!.Completed.Should().BeTrue("the assignment should be marked as complete");
            updatedAssignment.CompletedDate.Should().NotBeNull("the assignment should have a completion date");

            // Additionally, since this is the only assignment in the checklist,
            // the checklist should now report IsComplete = true and 100% completion.
            var updatedChecklist = await ((ApplicationDbContext)context).WeeklyChecklists
                .Include(tc => tc.Assignments)
                .FirstOrDefaultAsync(tc => tc.Id == checklist.Id, cancellationToken);

            updatedChecklist.Should().NotBeNull();
            updatedChecklist!.IsComplete.Should().BeTrue("all assignments in the checklist are completed");
            updatedChecklist.CompletionPercentage.Should().Be(100);
        }

        [Fact]
        public async Task Handle_Should_Return_Error_When_Assignment_Not_Found()
        {
            // Arrange
            var context = CreateInMemoryContext();
            Guid testUserId = Guid.NewGuid();

            // Act: Attempt to complete an assignment that does not exist.
            var command = new CompleteAssignmentItemCommand(Guid.NewGuid());
            var handler = new CompleteAssignmentItemCommandHandler(context);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert: Verify that the result is a failure.
            result.IsSuccess.Should().BeFalse("completing a non-existent assignment should fail");
            result.Error.Should().Be(TemplateErrors.TemplateNotFound(command.AssignmentId));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Workouts.Create;
using Domain.Workouts;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using SharedKernel;
using Application.Abstractions.Data;
using Domain.Templates;

namespace Tests
{
    public class CreateWorkoutTemplateCommandHandlerTests
    {

        private IApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options, new FakePublisher());
        }

        [Fact]
        public async Task Handle_Should_Create_WorkoutTemplate_Successfully()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var handler = new CreateWorkoutTemplateCommandHandler(context);
            Guid testUserId = Guid.NewGuid();
            var command = new CreateWorkoutTemplateCommand(
                "Morning Workout",
                "A workout to start the day",
                new List<WorkoutExerciseRequest>
                {
                    new WorkoutExerciseRequest("Push Up", 3, "10-15"),
                    new WorkoutExerciseRequest("Squat", 3, "10-15")
                },
                testUserId
            );
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert
            result.IsSuccess.Should().BeTrue("the workout template should be created successfully");
            Guid workoutId = result.Value;
            var workout = await ((ApplicationDbContext)context).WorkoutTemplates
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.Id == workoutId, cancellationToken);

            workout.Should().NotBeNull();
            workout!.Name.Should().Be("Morning Workout");
            workout.Description.Should().Be("A workout to start the day");
            workout.UserId.Should().Be(testUserId);
            workout.Exercises.Should().HaveCount(2);
            workout.Exercises.First().ExerciseName.Should().Be("Push Up");
        }

        [Fact]
        public async Task Handle_Should_Return_Error_When_TemplateName_Not_Unique()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var handler = new CreateWorkoutTemplateCommandHandler(context);
            Guid testUserId = Guid.NewGuid();

            var command = new CreateWorkoutTemplateCommand(
                "Duplicate Workout",
                "First instance",

                new List<WorkoutExerciseRequest>
                {
                    new WorkoutExerciseRequest("Exercise1", 3, "10-15")
                },
                testUserId
            );
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var firstResult = await handler.Handle(command, cancellationToken);
            var duplicateResult = await handler.Handle(command, cancellationToken);

            // Assert
            firstResult.IsSuccess.Should().BeTrue("the first template creation should succeed");
            duplicateResult.IsSuccess.Should().BeFalse("a duplicate template name should cause failure");
            duplicateResult.Error.Should().Be(TemplateErrors.TemplateNameNotUnique);
        }
    }
}

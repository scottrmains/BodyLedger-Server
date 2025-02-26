using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Application.Abstractions.Data;
using Application.Users.Update;

namespace Tests
{

    public class UserTests
    {
        private IApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options, new FakePublisher());
        }


        [Fact]
        public async Task Handle_Should_Create_Profile_If_Not_Exists()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var handler = new UpdateProfileCommandHandler(context);
            var userId = Guid.NewGuid();
            var command = new UpdateProfileCommand(userId, 75.5, 70.0, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(4.5));
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert
            result.IsSuccess.Should().BeTrue("a new profile should be created");
            var profile = await ((ApplicationDbContext)context).UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
            profile.Should().NotBeNull();
            profile!.CurrentWeight.Should().Be(75.5);
            profile.GoalWeight.Should().Be(70.0);
            profile.CurrentPace.Should().Be(TimeSpan.FromMinutes(5));
            profile.GoalPace.Should().Be(TimeSpan.FromMinutes(4.5));
 
        }

        [Fact]
        public async Task Handle_Should_Update_Existing_Profile()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var userId = Guid.NewGuid();

            var existingProfile = new UserProfile
            {
                UserId = userId,
                CurrentWeight = 80.0,
                GoalWeight = 75.0,
                CurrentPace = TimeSpan.FromMinutes(6),
                GoalPace = TimeSpan.FromMinutes(5.5)
            };
            context.UserProfiles.Add(existingProfile);
            await context.SaveChangesAsync(CancellationToken.None);

            var handler = new UpdateProfileCommandHandler(context);
            var command = new UpdateProfileCommand(userId, 78.0, null, TimeSpan.FromMinutes(5.2), TimeSpan.FromMinutes(5.5));
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert
            result.IsSuccess.Should().BeTrue("existing profile should be updated");
            var profile = await ((ApplicationDbContext)context).UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
            profile.Should().NotBeNull();
            profile!.CurrentWeight.Should().Be(78.0);
            profile.GoalWeight.Should().BeNull("the goal weight should have been removed");
            profile.CurrentPace.Should().Be(TimeSpan.FromMinutes(5.2));
            profile.GoalPace.Should().Be(TimeSpan.FromMinutes(5.5));
        }

        [Fact]
        public async Task Handle_Should_Allow_Removing_Values()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var userId = Guid.NewGuid();

            var existingProfile = new UserProfile
            {
                UserId = userId,
                CurrentWeight = 85.0,
                GoalWeight = 80.0,
                CurrentPace = TimeSpan.FromMinutes(4.8),
                GoalPace = TimeSpan.FromMinutes(4.5),
            };
            context.UserProfiles.Add(existingProfile);
            await context.SaveChangesAsync(CancellationToken.None);

            var handler = new UpdateProfileCommandHandler(context);
            var command = new UpdateProfileCommand(userId, null, null, null, null);
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert
            result.IsSuccess.Should().BeTrue("profile should update even when all values are removed");
            var profile = await ((ApplicationDbContext)context).UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
            profile.Should().NotBeNull();
            profile!.CurrentWeight.Should().BeNull();
            profile.GoalWeight.Should().BeNull();
            profile.CurrentPace.Should().BeNull();
            profile.GoalPace.Should().BeNull();

        }
    }
}

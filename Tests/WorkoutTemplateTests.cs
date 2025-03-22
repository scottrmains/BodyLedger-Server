
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;


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


    }
}


using Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;
using Application.Abstractions.Services;

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


    }
}

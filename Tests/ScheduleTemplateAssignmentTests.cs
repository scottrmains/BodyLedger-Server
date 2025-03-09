
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

using Domain.Checklists;

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

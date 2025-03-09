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
using Domain.Checklists;

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


    }
}

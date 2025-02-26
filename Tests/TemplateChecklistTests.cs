using System;
using System.Threading;
using System.Threading.Tasks;
using Application.TemplateChecklists.Create;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions; // Optional, for fluent assertions
using SharedKernel;
using Application.Abstractions.Data;
using Domain.Templates;


namespace Tests;
public class FakePublisher : IPublisher
{
    public Task Publish(object notification, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    Task IPublisher.Publish<TNotification>(TNotification notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}


public class CreateTemplateChecklistCommandHandlerTests
{
    // Helper method to create an in-memory ApplicationDbContext
    private IApplicationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options, new FakePublisher());
    }


}

using Application.Abstractions.Data;
using Domain.Assignments;
using Domain.Checklists;
using Domain.TemplateAssignments;
using Domain.Templates;
using Domain.Templates.Fitness;
using Domain.Users;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedKernel;

namespace Infrastructure.Database;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    // Templates
    public DbSet<Template> Templates { get; set; }
    public DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }
    public DbSet<FitnessTemplate> FitnessTemplates { get; set; }

    // Activities
    public DbSet<WorkoutActivity> WorkoutActivities { get; set; }
    public DbSet<FitnessActivity> FitnessActivities { get; set; }

    // Assignments
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignmentItem> AssignmentItems { get; set; }
    public DbSet<WorkoutActivityAssignment> WorkoutActivityAssignments { get; set; }
    public DbSet<FitnessActivityAssignment> FitnessActivityAssignments { get; set; }

    // Checklists
    public DbSet<Checklist> Checklists { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // When should you publish domain events?
        //
        // 1. BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // 2. AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail

        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
    {
        return base.Entry(entity);
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}

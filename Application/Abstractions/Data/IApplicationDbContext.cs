
using Domain.Assignments;
using Domain.Checklists;
using Domain.TemplateAssignments;
using Domain.Templates;
using Domain.Users;
using Domain.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }
    DbSet<WorkoutExercise> WorkoutExercises { get; set; }

    DbSet<Assignment> Assignments { get; set; }
    DbSet<AssignmentItem> AssignmentItems { get; set; }
    DbSet<Checklist> Checklists { get; set; }
    DbSet<Template> Templates { get; set; }
    DbSet<WorkoutExerciseAssignment> WorkoutExerciseAssignments { get; set; }

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

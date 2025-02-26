
using Domain.Assignments;
using Domain.Checklist;
using Domain.Templates;
using Domain.Users;
using Domain.Workouts;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }
    DbSet<WorkoutExercise> WorkoutExercises { get; set; }

    DbSet<TemplateAssignment> TemplateAssignments { get; set; }
    DbSet<AssignmentItem> AssignmentItems { get; set; }
    DbSet<WeeklyChecklist> WeeklyChecklists { get; set; }
    DbSet<Template> Templates { get; set; } 


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

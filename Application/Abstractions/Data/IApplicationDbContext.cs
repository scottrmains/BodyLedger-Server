﻿using Domain.Assignments;
using Domain.Checklists;
using Domain.TemplateAssignments;
using Domain.Templates;
using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Domain.Templates.Fitness;

namespace Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<UserProfile> UserProfiles { get; }

        // Templates
        DbSet<Template> Templates { get; }
        DbSet<WorkoutTemplate> WorkoutTemplates { get; }
        DbSet<FitnessTemplate> FitnessTemplates { get; }

        // Activities
        DbSet<WorkoutActivity> WorkoutActivities { get; }
        DbSet<FitnessActivity> FitnessActivities { get; }

        // Assignments
        DbSet<Assignment> Assignments { get; }
        DbSet<AssignmentItem> AssignmentItems { get; }
        DbSet<WorkoutActivityAssignment> WorkoutActivityAssignments { get; }
        DbSet<FitnessActivityAssignment> FitnessActivityAssignments { get; }

        // Checklists
        DbSet<Checklist> Checklists { get; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
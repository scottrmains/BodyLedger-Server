﻿using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Templates.GetAllByUserId;
using Application.Templates.Mapping;
using Application.Workouts.GetAllByUserId;
using Domain.Templates;
using Domain.Templates.Fitness;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Templates.GetByUserId
{
    internal sealed class GetTemplatesByUserIdQueryHandler(
        IApplicationDbContext context,
        IUserContext userContext, TemplateMapper mapper)
        : IQueryHandler<GetTemplatesByUserIdQuery, TemplateListResponse>
    {
        public async Task<Result<TemplateListResponse>> Handle(GetTemplatesByUserIdQuery query, CancellationToken cancellationToken)
        {
            if (query.UserId != userContext.UserId)
            {
                return Result.Failure<TemplateListResponse>(UserErrors.Unauthorized());
            }

            // Build base query with template type filter if specified
            var templatesQuery = context.Templates
                .Where(t => t.UserId == query.UserId);

            if (query.TemplateType.HasValue)
            {
                string discriminatorValue = query.TemplateType.Value.ToString().ToLowerInvariant();
                templatesQuery = templatesQuery.Where(t => EF.Property<string>(t, "template_type") == discriminatorValue);
            }

            // Get total count for pagination
            int totalTemplates = await templatesQuery.CountAsync(cancellationToken);

            // Get paginated templates
            var templates = await templatesQuery
                .OrderByDescending(t => t.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            // Load related data only for template types that need it
            Dictionary<Guid, List<WorkoutActivity>> workoutActivities = new();
            Dictionary<Guid, List<FitnessActivity>> fitnessActivities = new();
            // Only load workout exercises if we have workout templates
            var workoutTemplates = templates.OfType<WorkoutTemplate>().ToList();
            if (workoutTemplates.Any())
            {
                var workoutIds = workoutTemplates.Select(w => w.Id).ToList();
                workoutActivities = await context.WorkoutActivities
                    .Where(e => workoutIds.Contains(e.WorkoutTemplateId))
                    .GroupBy(e => e.WorkoutTemplateId)
                    .ToDictionaryAsync(
                        g => g.Key,
                        g => g.ToList(),
                        cancellationToken);
            }

            var fitnessTemplates = templates.OfType<FitnessTemplate>().ToList();

            if (fitnessTemplates.Any())
            {
                var fitnessIds = fitnessTemplates.Select(w => w.Id).ToList();
                fitnessActivities = await context.FitnessActivities
                    .Where(e => fitnessIds.Contains(e.FitnessTemplateId))
                    .GroupBy(e => e.FitnessTemplateId)
                    .ToDictionaryAsync(
                        g => g.Key,
                        g => g.ToList(),
                        cancellationToken);
            }

            // Map templates to response objects
            var templateResponses = templates
                .Select(t => mapper.MapTemplateToResponse(t, workoutActivities, fitnessActivities))
                .ToList();

            // Create paginated response
            var response = new TemplateListResponse
            {
                Items = templateResponses,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalTemplates
            };

            return Result.Success(response);
        }


    }
}
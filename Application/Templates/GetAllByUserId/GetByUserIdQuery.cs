using Application.Abstractions.Messaging;
using Application.Workouts.GetAllByUserId;
using Domain.Templates;
using SharedKernel.Enums;


namespace Application.Templates.GetAllByUserId;

public sealed record GetTemplatesByUserIdQuery(
      Guid UserId,
      int Page,
      int PageSize,
      TemplateType? TemplateType = null) : IQuery<TemplateListResponse>;

using Application.Abstractions.Messaging;
using SharedKernel.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Checklists.GetByUserId;

public sealed record GetChecklistByUserIdQuery(Guid UserId, DateTime? ReferenceDate = null) : IQuery<ChecklistResponse>;

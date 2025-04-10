using Application.Abstractions.Messaging;
using SharedKernel.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Progress
{
    public sealed record GetMonthlyProgressQuery(
       Guid UserId,
       int Year,
       int Month) : IQuery<MonthlyProgressResponse>;
}

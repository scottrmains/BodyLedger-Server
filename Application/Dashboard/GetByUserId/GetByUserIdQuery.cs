using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.GetByUserId
{
    public sealed record GetDashboardByUserIdQuery(Guid UserId, DateTime? ReferenceDate = null) : IQuery<DashboardResponse>;

}

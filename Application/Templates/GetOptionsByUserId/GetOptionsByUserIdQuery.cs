using Application.Abstractions.Messaging;
using Application.Dashboard.GetByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Templates.GetOptionsByUserId
{
    public sealed record GetOptionsByUserIdQuery(Guid UserId) : IQuery<TemplateOptionsResponse>;
}

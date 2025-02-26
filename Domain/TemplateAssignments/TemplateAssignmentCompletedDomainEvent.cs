using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Assignments
{
    public sealed record TemplateAssignmentCompletedDomainEvent(Guid AssignmentId) : IDomainEvent;
}

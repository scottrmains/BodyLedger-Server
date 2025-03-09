using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Checklists;

    public sealed record ChecklistCreatedDomainEvent(Guid ChecklistId) : IDomainEvent;


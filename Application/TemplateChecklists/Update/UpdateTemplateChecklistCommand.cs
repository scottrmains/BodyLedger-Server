using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TemplateChecklists.Update;

    public sealed record UpdateTemplateChecklistCommand(Guid UserId, DayOfWeek NewStartDay) : ICommand<Guid>;


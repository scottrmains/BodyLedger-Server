using Application.Abstractions.Messaging;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Checklists.Update
{
    public sealed record UpdateChecklistLogCommand(
          Guid UserId,
          Guid ChecklistId,
          double? Weight,
          string Notes,
          MoodType Mood) : ICommand;
}

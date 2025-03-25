using Application.Abstractions.Messaging;
using Application.Templates.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Templates.Generate
{
    public sealed record GenerateWorkoutTemplateCommand(
      string Name,
      string Description,
      Guid UserId) : ICommand<List<WorkoutActivityResponse>>;
}

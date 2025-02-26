using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.DataTransferObjects
{
    public sealed record DailyAssignmentMapping(
      DayOfWeek Day,
      Guid TemplateId,
      bool IsRecurring
  );

}

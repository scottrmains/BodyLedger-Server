using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{

    public class TemplateDateHelper
    {
        public static DateTime GetStartLimit(DateTime? lastStartDate, DayOfWeek preferredStartDay)
        {
            DateTime today = DateTime.UtcNow.Date;
            int daysSinceLastMonday = (int)today.DayOfWeek - (int)preferredStartDay;
            if (daysSinceLastMonday < 0)
            {
                daysSinceLastMonday += 7;
            }
            DateTime startDate = today.AddDays(-daysSinceLastMonday).AddDays(7);
            return startDate;
        }

        public static string GetDayNameFromOffset(int offset, int cycleStartDay)
        {
            int adjustedDay = (cycleStartDay + offset) % 7; 
            return Enum.GetName(typeof(DayOfWeek), adjustedDay) ?? "Unknown";
        }

        public static bool IsPastDayInCycle(DayOfWeek scheduled, DayOfWeek today, DayOfWeek cycleStart)
        {
            // Normalize both days relative to the cycle start
            int normalizedScheduled = ((int)scheduled - (int)cycleStart + 7) % 7;
            int normalizedToday = ((int)today - (int)cycleStart + 7) % 7;
            return normalizedScheduled < normalizedToday;
        }

        public static DateTime GetCycleStartDate(DateTime referenceDate, DayOfWeek startDay)
        {
            int diff = ((int)referenceDate.DayOfWeek - (int)startDay + 7) % 7;
            return referenceDate.Date.AddDays(-diff);
        }

        public static DateTime GetCycleStartForReference(DateTime referenceDate, DayOfWeek startDay, int cycleOffset = 0)
        {
            int diff = ((int)referenceDate.DayOfWeek - (int)startDay + 7) % 7;
            DateTime currentCycleStart = referenceDate.Date.AddDays(-diff);
            return currentCycleStart.AddDays(7 * cycleOffset);
        }


        public static int NormalizeDay(DayOfWeek day, DayOfWeek startDay)
        {
            return ((int)day - (int)startDay + 7) % 7;
        }

    }

}

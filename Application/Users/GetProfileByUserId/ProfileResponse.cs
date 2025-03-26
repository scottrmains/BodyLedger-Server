using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetProfileByUserId
{
    public sealed record ProfileResponse
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double? CurrentWeight { get; set; } 
        public double? GoalWeight { get; set; }
        public TimeSpan? CurrentPace {  get; set; }
        public TimeSpan? GoalPace { get; set; }
    }
}

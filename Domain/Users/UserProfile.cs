using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users;

        public class UserProfile : Entity
        {
            public Guid UserId { get; set; }
            public User User { get; set; }
            public double? CurrentWeight { get; set; }
            public double? GoalWeight { get; set; }
            public TimeSpan? CurrentPace { get; set; }
            public TimeSpan? GoalPace { get; set; }
      
        }
    



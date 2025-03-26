using Domain.Users;
using SharedKernel;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Templates
{
    public abstract class Template : Entity
    {
        public string Name { get; set; }
        public TemplateType Type { get; protected set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; protected set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; protected set; } = DateTime.UtcNow;



        public Guid UserId { get; set; }
        public virtual User User { get; set; }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Templates.GetOptionsByUserId
{
    public sealed class TemplateOptionsResponse
    {
        public Dictionary<Guid, string> TemplateNames { get; set; }

    }
}

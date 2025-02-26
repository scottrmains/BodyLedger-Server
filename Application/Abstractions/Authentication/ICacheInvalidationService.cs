using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Authentication
{
    public interface ICacheInvalidationService
    {
        void InvalidateByPrefix(string prefix);
    }
}

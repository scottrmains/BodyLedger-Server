using SharedKernel.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IProgressService
    {
        Task<MonthlyProgressResponse> GetMonthlyProgressAsync(
            Guid userId,
            int year,
            int month,
            CancellationToken cancellationToken);
    }
}

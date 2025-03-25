using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IAiService
    {
        Task<TResult> GenerateContent<TResult>(
             string prompt,
             JsonSerializerOptions? jsonOptions = null,
             double temperature = 0.7,
             int maxOutputTokens = 8192,
             CancellationToken cancellationToken = default);
    }


    public class GeneratedExercise
    {
        public string Name { get; set; }
        public int Sets { get; set; }
        public string RepRange { get; set; }
    }
}

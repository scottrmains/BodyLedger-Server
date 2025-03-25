using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedKernel.Responses
{
    public sealed class NotificationResponse
    {

        public Guid UserId { get; init; }
        public string Title { get; init; }
        public string Message { get; init; }
        public int Type { get; init; }
        public bool IsRead { get; init; }
        public DateTime CreatedAt { get; init; }
        public JsonDocument? Metadata { get; init; }
    }
}

using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public enum NotificationType
    {
        AssignmentCompleted = 1,
        Message = 2,
        Reminder = 3,
        Achievement = 4,
        System = 5
    }

    public class Notification : Entity
    {
        public Guid UserId { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public NotificationType Type { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public JsonDocument? Metadata { get; private set; }

        private Notification() { }

        public Notification(
            Guid userId,
            string title,
            string message,
            NotificationType type,
            JsonDocument? metadata = null)
        {
            UserId = userId;
            Title = title;
            Message = message;
            Type = type;
            IsRead = false;
            CreatedAt = DateTime.UtcNow;
            Metadata = metadata;
        }

        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                Raise(new NotificationReadDomainEvent(Id));
            }
        }

        public static Notification CreateAssignmentCompletedNotification(
            Guid userId,
            string title,
            string message,
            Guid assignmentId,
            Guid? templateId = null)
        {
            var metadata = JsonDocument.Parse(JsonSerializer.Serialize(new
            {
                activityId = assignmentId.ToString(),
                templateId = templateId?.ToString()
            }));

            return new Notification(userId, title, message, NotificationType.AssignmentCompleted, metadata);
        }

        public static Notification CreateReminderNotification(
            Guid userId,
            string title,
            string message,
            Guid activityId,
            Guid? templateId = null)
        {
            var metadata = JsonDocument.Parse(JsonSerializer.Serialize(new
            {
                activityId = activityId.ToString(),
                templateId = templateId?.ToString()
            }));

            return new Notification(userId, title, message, NotificationType.Reminder, metadata);
        }

        public static Notification CreateAchievementNotification(
            Guid userId,
            string title,
            string message,
            Guid achievementId)
        {
            var metadata = JsonDocument.Parse(JsonSerializer.Serialize(new
            {
                achievementId = achievementId.ToString()
            }));

            return new Notification(userId, title, message, NotificationType.Achievement, metadata);
        }

        public static Notification CreateMessageNotification(
            Guid userId,
            string title,
            string message,
            Guid? fromUserId = null)
        {
            var metadata = fromUserId.HasValue
                ? JsonDocument.Parse(JsonSerializer.Serialize(new { fromUserId = fromUserId.Value.ToString() }))
                : null;

            return new Notification(userId, title, message, NotificationType.Message, metadata);
        }

        public static Notification CreateSystemNotification(
            Guid userId,
            string title,
            string message,
            object? metadataObj = null)
        {
            JsonDocument? metadata = null;
            if (metadataObj != null)
            {
                metadata = JsonDocument.Parse(JsonSerializer.Serialize(metadataObj));
            }

            return new Notification(userId, title, message, NotificationType.System, metadata);
        }
    }
}

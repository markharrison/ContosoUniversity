using System.Collections.Concurrent;
using ContosoUniversity.Models;

namespace ContosoUniversity.Services
{
    public class NotificationService
    {
        private readonly ConcurrentQueue<Notification> _notifications = new();

        public void SendNotification(string entityType, string entityId, EntityOperation operation, string userName = null)
        {
            SendNotification(entityType, entityId, null, operation, userName);
        }

        public void SendNotification(string entityType, string entityId, string entityDisplayName, EntityOperation operation, string userName = null)
        {
            var displayText = !string.IsNullOrWhiteSpace(entityDisplayName)
                ? $"{entityType} '{entityDisplayName}'"
                : $"{entityType} (ID: {entityId})";

            _notifications.Enqueue(new Notification
            {
                EntityType = entityType,
                EntityId = entityId,
                Operation = operation.ToString(),
                Message = operation switch
                {
                    EntityOperation.CREATE => $"New {displayText} has been created",
                    EntityOperation.UPDATE => $"{displayText} has been updated",
                    EntityOperation.DELETE => $"{displayText} has been deleted",
                    _ => $"{displayText} operation: {operation}"
                },
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userName ?? "System"
            });
        }

        public Notification ReceiveNotification()
        {
            return _notifications.TryDequeue(out var notification) ? notification : null;
        }

        public void MarkAsRead(int notificationId)
        {
        }
    }
}

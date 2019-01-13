namespace Reacher.Notification.Pushover
{
    public interface INotificationPushoverService
    {
        void Send(string title, string message);
    }
}
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseBasicMessageHandler : MonoBehaviour
    {
        public abstract void ShowMessage(string message);
    }

    public enum MessageType { Notification, Error }
    public abstract class BaseMessageHandler : MonoBehaviour
    {
        public abstract void ShowMessage(string message, MessageType type = MessageType.Notification);
    }

    public class MessageHandler : BaseMessageHandler
    {
        public BaseBasicMessageHandler NotificationHandler { get => notificationHandler; set => notificationHandler = value; }
        [SerializeField] private BaseBasicMessageHandler notificationHandler;
        public BaseBasicMessageHandler ErrorHandler { get => errorHandler; set => errorHandler = value; }
        [SerializeField] private BaseBasicMessageHandler errorHandler;


        public override void ShowMessage(string message, MessageType type = MessageType.Notification)
        {
            BaseBasicMessageHandler messageHandler;
            if (type == MessageType.Notification)
                messageHandler = NotificationHandler;
            else
                messageHandler = ErrorHandler;
            messageHandler.ShowMessage(message);
        }
    }
}
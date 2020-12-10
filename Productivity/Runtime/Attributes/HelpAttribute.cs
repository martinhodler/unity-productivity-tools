using System;

namespace UnityEngine.Productivity.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HelpAttribute : PropertyAttribute
    {
        public enum MessageType
        {
            None,
            Info,
            Warning,
            Error
        }


        public string Message { get; }
        public MessageType Type { get; }

        public HelpAttribute(string message, MessageType type = MessageType.Info)
        {
            Message = message;
            Type = type;
        }
    }
}
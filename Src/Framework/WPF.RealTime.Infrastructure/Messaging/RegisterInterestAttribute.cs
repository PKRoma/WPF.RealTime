using System;

namespace WPF.RealTime.Infrastructure.Messaging
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RegisterInterestAttribute : Attribute
    {
        public string Topic { get; private set; }

        public TaskType TaskType { get; private set; }

        public RegisterInterestAttribute(string topic, TaskType taskType)
        {
            Topic = topic;
            TaskType = taskType;
        }
    }
}

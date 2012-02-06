using System;

namespace WPF.RealTime.Infrastructure
{
    public sealed class ViewAttribute : Attribute
    {
        public Type ViewType { get; private set; }

        public ViewAttribute(Type viewType)
        {
            ViewType = viewType;
        }
    }
}

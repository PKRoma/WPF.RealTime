using System;

namespace WPF.RealTime.Infrastructure.Interfaces
{
    public interface IDispatcherFacade
    {
        void AddToDispatcherQueue(Delegate workItem);
    }
}

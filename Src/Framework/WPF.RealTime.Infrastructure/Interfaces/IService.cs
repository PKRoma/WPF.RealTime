using System;
using WPF.RealTime.Data;

namespace WPF.RealTime.Infrastructure.Interfaces
{
    public interface IService
    {
        void GetData();
        event EventHandler<EventArgs<DataRecord>> DataReceived;
        AssetType AssetType { get; }
    }
}

using System.ServiceModel;

namespace WPF.RealTime.Data.Interfaces
{
    [ServiceContract]
    public interface IRemoteSubscriptionService
    {
        [OperationContract(IsOneWay = true)]
        void GetData(RequestRecord request);
    }
}

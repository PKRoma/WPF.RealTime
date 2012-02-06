using System.ServiceModel;

namespace WPF.RealTime.Data.Interfaces
{
    [ServiceContract]
    public interface IRemotePublishingService
    {
        [OperationContract(IsOneWay = true)]
        void DataSetRecordsChanged(DataRecord data);
    }
}

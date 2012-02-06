using System;
using System.ServiceModel;
using WPF.RealTime.Data;
using WPF.RealTime.Data.Interfaces;

namespace WPF.RealTime.Server
{
    public class WcfServer : IDisposable
    {
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class ConnectionListener : IRemoteSubscriptionService
        {
            private readonly ChannelFactory<IRemotePublishingService> _channelFutureFactory;
            private readonly IRemotePublishingService _futureProxy;

            private readonly ChannelFactory<IRemotePublishingService> _channelBondFactory;
            private readonly IRemotePublishingService _bondFuture;

            public ConnectionListener()
            {
                _channelFutureFactory = new ChannelFactory<IRemotePublishingService>(
                        new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = 5000000, MaxBufferSize = 5000000 },
                        "net.pipe://WPFRealTime/Future/PublishingService");
                _futureProxy = _channelFutureFactory.CreateChannel();

                _channelBondFactory = new ChannelFactory<IRemotePublishingService>(
                    new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = 5000000, MaxBufferSize = 5000000 },
                    "net.pipe://WPFRealTime/Bond/PublishingService");
                _bondFuture = _channelBondFactory.CreateChannel();
            }

            #region IRemoteSubscriptionService Members

            public void GetData(RequestRecord requestRecord)
            {
                Console.WriteLine("GetData called");
                switch (requestRecord.AssetType)
                {
                    case AssetType.Bond:
                        BondServer.GetData(_bondFuture);
                        break;
                    case AssetType.Future:
                        FutureServer.GetData(_futureProxy);
                        break;
                    default:
                        break;
                }
            }

            #endregion
        }

        private readonly ServiceHost _host;
        public WcfServer()
        {
            _host = new ServiceHost(new ConnectionListener());
            _host.AddServiceEndpoint(typeof(IRemoteSubscriptionService),
                new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = 5000000, MaxBufferSize = 5000000 },
                "net.pipe://WPFRealTime/SubscriptionService");
            _host.Open();
        }

        #region IDisposable Members

        public void Dispose()
        {
            _host.Close();
        }

        #endregion
    }
}

using System;
using System.ComponentModel.Composition;
using System.Threading;
using WPF.RealTime.Data;
using WPF.RealTime.Data.ResourceManager;
using WPF.RealTime.Infrastructure;
using WPF.RealTime.Infrastructure.Interfaces;
using WPF.RealTime.Infrastructure.Messaging;

namespace FutureService.Implementation
{
    [Export("MOCK", typeof(IService))]
    public class MockService : IService
    {
        private readonly Random _rand = new Random();

        #region IService Members
        [RegisterInterest(Topic.FutureServiceGetData, TaskType.Background)]
        public void GetData()
        {
            //ThreadPool.QueueUserWorkItem
            //    (
            //       _=>
            {
                for (; ; )
                {
                    //Thread.Sleep(1);
                    var key = "FUTURE" + _rand.Next(1, 20);
                    int prop = _rand.Next(1, 21);
                    string propName = string.Empty;
                    object propValue = null;
                    switch (prop)
                    {
                        case 1:
                            propName = "BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 2:
                            propName = "AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 3:
                            propName = "BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 4:
                            propName = "AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 5:
                            propName = "AskVol";
                            propValue = _rand.Next();
                            break;
                        case 6:
                            propName = "BidVol";
                            propValue = _rand.Next();
                            break;
                        case 7:
                            propName = "BasisAsk";
                            propValue = _rand.NextDouble();
                            break;
                        case 8:
                            propName = "BasisAskSize";
                            propValue = _rand.NextDouble();
                            break;
                        case 9:
                            propName = "BasisAskChange";
                            propValue = _rand.NextDouble();
                            break;
                        case 10:
                            propName = "BasisBid";
                            propValue = _rand.NextDouble();
                            break;
                        case 11:
                            propName = "BasisBidSize";
                            propValue = _rand.NextDouble();
                            break;
                        case 12:
                            propName = "BasisBidChange";
                            propValue = _rand.NextDouble();
                            break;
                        case 13:
                            propName = "CM1BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 14:
                            propName = "CM1AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 15:
                            propName = "CM1BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 16:
                            propName = "CM1AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 17:
                            propName = "CM2BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 18:
                            propName = "CM2AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 19:
                            propName = "CM2BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 20:
                            propName = "CM2AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        /*
                        case 21:
                            propName = "CM3BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 22:
                            propName = "CM3AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 23:
                            propName = "CM3BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 24:
                            propName = "CM3AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 25:
                            propName = "CM4BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 26:
                            propName = "CM4AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 27:
                            propName = "CM4BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 28:
                            propName = "CM4AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 29:
                            propName = "CM5BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 30:
                            propName = "CM5AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 31:
                            propName = "CM5BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 32:
                            propName = "CM5AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 33:
                            propName = "FM1BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 34:
                            propName = "FM1AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 35:
                            propName = "FM1BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 36:
                            propName = "FM1AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 37:
                            propName = "FM2BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 38:
                            propName = "FM2AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 39:
                            propName = "FM2BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 40:
                            propName = "FM2AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 41:
                            propName = "FM3BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 42:
                            propName = "FM3AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 43:
                            propName = "FM3AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 44:
                            propName = "FM3BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 45:
                            propName = "FM4BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 46:
                            propName = "FM4AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 47:
                            propName = "FM4BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 48:
                            propName = "FM4AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 49:
                            propName = "FM5BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 50:
                            propName = "FM5AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 51:
                            propName = "FM5BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 52:
                            propName = "FM5AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 53:
                            propName = "FwBidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 54:
                            propName = "FwMidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 55:
                            propName = "FwAskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 56:
                            propName = "ExpiryDate";
                            propValue = DateTime.UtcNow;
                            break;
                        */
                    }
                    EventHandler<EventArgs<DataRecord>> handler = DataReceived;
                    if (handler != null)
                    {
                        handler(this, new EventArgs<DataRecord>(new DataRecord(key, propName, propValue)));
                    }
                }
            }
            //);
        }

        public event EventHandler<EventArgs<DataRecord>> DataReceived;

        public AssetType AssetType { get { return AssetType.Future; } }
        #endregion


    }
}

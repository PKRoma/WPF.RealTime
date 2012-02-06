using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading;
using WPF.RealTime.Data;
using WPF.RealTime.Data.ResourceManager;
using WPF.RealTime.Infrastructure;
using WPF.RealTime.Infrastructure.Interfaces;
using WPF.RealTime.Infrastructure.Messaging;
using WPF.RealTime.Infrastructure.Utils;

namespace BondService.Implementation
{
    [Export("MOCK", typeof(IService))]
    public class MockService : IService
    {
        private readonly Random _rand = new Random();
        private readonly bool _isProcessorAffinity =
                                Convert.ToBoolean(ConfigurationManager.AppSettings["PROCESSOR_AFFINITY"]);

        #region IService Members
        [RegisterInterest(Topic.BondServiceGetData, TaskType.Background)]
        public void GetData()
        {
            //ThreadPool.QueueUserWorkItem
            //    (
            //       _=>
            {
                for (; ; )
                {
                    if (_isProcessorAffinity) ProcessorAffinity.BeginAffinity(0);
                    //Thread.Sleep(1);
                    var key = String.Intern("BOND" + _rand.Next(1, 30));
                    int prop = _rand.Next(1, 56);
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
                            propName = "M1BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 34:
                            propName = "M1AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 35:
                            propName = "M1BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 36:
                            propName = "M1AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 37:
                            propName = "M2BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 38:
                            propName = "M2AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 39:
                            propName = "M2BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 40:
                            propName = "M2AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 41:
                            propName = "M3BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 42:
                            propName = "M3AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 43:
                            propName = "M3AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 44:
                            propName = "M3BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 45:
                            propName = "M4BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 46:
                            propName = "M4AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 47:
                            propName = "M4BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 48:
                            propName = "M4AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 49:
                            propName = "M5BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 50:
                            propName = "M5AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 51:
                            propName = "M5BidYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 52:
                            propName = "M5AskYield";
                            propValue = _rand.NextDouble();
                            break;
                        case 53:
                            propName = "SpreadBidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 54:
                            propName = "SpreadMidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 55:
                            propName = "SpreadAskPrice";
                            propValue = _rand.NextDouble();
                            break;

                    }
                    EventHandler<EventArgs<DataRecord>> handler = DataReceived;
                    if (handler != null)
                    {
                        handler(this, new EventArgs<DataRecord>(new DataRecord(key, String.Intern(propName), propValue)));
                    }
                }
            }
            //);
        }

        public event EventHandler<EventArgs<DataRecord>> DataReceived;

        public AssetType AssetType { get { return AssetType.Bond; } }

        #endregion


    }
}

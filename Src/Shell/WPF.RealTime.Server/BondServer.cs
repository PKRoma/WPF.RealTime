using System;
using System.Configuration;
using System.Threading.Tasks;
using WPF.RealTime.Data;
using WPF.RealTime.Data.Interfaces;

namespace WPF.RealTime.Server
{
    public class BondServer
    {
        private static readonly Random Rand = new Random();
        private static readonly bool IsProcessorAffinity =
                                Convert.ToBoolean(ConfigurationManager.AppSettings["PROCESSOR_AFFINITY"]);

        public static void GetData(IRemotePublishingService proxy)
        {
            Task.Factory.StartNew(() =>
            {
                {
                    for (; ; )
                    {
                        if (IsProcessorAffinity) ProcessorAffinity.BeginAffinity(0);

                        var key = String.Intern("BOND" + Rand.Next(1, 300));
                        int prop = Rand.Next(1, 56);
                        string propName = string.Empty;
                        object propValue = null;
                        switch (prop)
                        {
                            case 1:
                                propName = "BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 2:
                                propName = "AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 3:
                                propName = "BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 4:
                                propName = "AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 5:
                                propName = "AskVol";
                                propValue = Rand.Next();
                                break;
                            case 6:
                                propName = "BidVol";
                                propValue = Rand.Next();
                                break;
                            case 7:
                                propName = "BasisAsk";
                                propValue = Rand.NextDouble();
                                break;
                            case 8:
                                propName = "BasisAskSize";
                                propValue = Rand.NextDouble();
                                break;
                            case 9:
                                propName = "BasisAskChange";
                                propValue = Rand.NextDouble();
                                break;
                            case 10:
                                propName = "BasisBid";
                                propValue = Rand.NextDouble();
                                break;
                            case 11:
                                propName = "BasisBidSize";
                                propValue = Rand.NextDouble();
                                break;
                            case 12:
                                propName = "BasisBidChange";
                                propValue = Rand.NextDouble();
                                break;
                            case 13:
                                propName = "CM1BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 14:
                                propName = "CM1AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 15:
                                propName = "CM1BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 16:
                                propName = "CM1AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 17:
                                propName = "CM2BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 18:
                                propName = "CM2AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 19:
                                propName = "CM2BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 20:
                                propName = "CM2AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 21:
                                propName = "CM3BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 22:
                                propName = "CM3AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 23:
                                propName = "CM3BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 24:
                                propName = "CM3AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 25:
                                propName = "CM4BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 26:
                                propName = "CM4AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 27:
                                propName = "CM4BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 28:
                                propName = "CM4AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 29:
                                propName = "CM5BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 30:
                                propName = "CM5AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 31:
                                propName = "CM5BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 32:
                                propName = "CM5AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 33:
                                propName = "M1BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 34:
                                propName = "M1AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 35:
                                propName = "M1BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 36:
                                propName = "M1AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 37:
                                propName = "M2BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 38:
                                propName = "M2AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 39:
                                propName = "M2BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 40:
                                propName = "M2AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 41:
                                propName = "M3BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 42:
                                propName = "M3AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 43:
                                propName = "M3AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 44:
                                propName = "M3BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 45:
                                propName = "M4BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 46:
                                propName = "M4AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 47:
                                propName = "M4BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 48:
                                propName = "M4AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 49:
                                propName = "M5BidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 50:
                                propName = "M5AskPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 51:
                                propName = "M5BidYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 52:
                                propName = "M5AskYield";
                                propValue = Rand.NextDouble();
                                break;
                            case 53:
                                propName = "SpreadBidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 54:
                                propName = "SpreadMidPrice";
                                propValue = Rand.NextDouble();
                                break;
                            case 55:
                                propName = "SpreadAskPrice";
                                propValue = Rand.NextDouble();
                                break;

                        }
                        proxy.DataSetRecordsChanged(new DataRecord(key, String.Intern(propName), propValue));
                    }
                }
            });
        }

    }
}

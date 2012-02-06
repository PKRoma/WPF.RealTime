using System;

namespace WPF.RealTime.Data
{
    [Serializable]
    public class Heartbeat
    {
        public string Key { get; private set; }
        public string Message { get; private set; }
        public DateTime TimeCreated { get; private set; }
        public bool NonRepeatable { get; private set; }

        public Heartbeat(string key, string message, DateTime time, bool nonRep)
        {
            Key = key;
            Message = message;
            TimeCreated = time;
            NonRepeatable = nonRep;
        }
    }
}

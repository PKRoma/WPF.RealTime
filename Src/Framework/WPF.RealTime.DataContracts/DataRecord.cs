using System;
using System.Runtime.Serialization;

namespace WPF.RealTime.Data
{
    [Serializable]
    [DataContract(Name = "DataRecord", Namespace = "net.pipe://WPFRealTime")]
    public class DataRecord
    {
        [DataMember(Name = "DataRecordKey")]
        public string DataRecordKey { get; private set; }
         [DataMember(Name = "PropertyName")]
        public string PropertyName { get; private set; }
         [DataMember(Name = "PropertyValue")]
        public object PropertyValue { get; private set; }

        public DataRecord(string key, string name, object value)
        {
            DataRecordKey = key;
            PropertyName = name;
            PropertyValue = value;
        }
    }

    [Serializable]
    [DataContract(Name = "RequestRecord", Namespace = "net.pipe://WPFRealTime")]
    public class RequestRecord
    {
        [DataMember(Name = "AssetType")]
        public AssetType AssetType { get; set; }
    }
}

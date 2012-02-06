using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Xml.Linq;

namespace WPF.RealTime.Data.Entities
{
    public sealed class EntityBuilder
    {
        internal static Dictionary<AssetType, EntityPropertyDescriptor[]> Metadata =
            new Dictionary<AssetType, EntityPropertyDescriptor[]>();

        // Fowler–Noll–Vo hash function
        // non-cryptographic string hash
        public static ulong Fnv1Hash(params string[] sources)
        {
            const ulong fnvOffsetBasis = 14695981039346656037;
            const ulong fnvPrime = 1099511628211;
            ulong hash = fnvOffsetBasis;
            foreach (var src in sources)
            {
                foreach (var c in src.ToCharArray())
                {
                    hash = hash * fnvPrime;
                    hash = hash ^ c;
                }
            }
            return hash;
        }

        public static PropertyDescriptorCollection LoadMetadata(params AssetType[] assets)
        {
            var descriptors = new List<PropertyDescriptor>();
            foreach (var assetType in assets)
            {
                foreach (var pd in Metadata[assetType])
                {
                    if (!descriptors.Contains(pd)) 
                        descriptors.Add(pd);
                }
            }

            return new PropertyDescriptorCollection(descriptors.ToArray(), false);
        }

        static EntityBuilder()
        {
            var mf = ConfigurationManager.AppSettings["METADATA_FILE"];
            XElement mdFile = XElement.Load(mf);

            var allDescriptors = new List<EntityPropertyDescriptor>();
            foreach (XElement node in mdFile.Elements())
            {
                var descriptors = new List<EntityPropertyDescriptor>();
                foreach (var e in node.Elements())
                {
                    var pd = new EntityPropertyDescriptor(
                            e.Attribute("Name").Value,
                            Convert.ToBoolean(e.Attribute("ReadOnly").Value),
                            Type.GetType(e.Attribute("Type").Value), null);
                    descriptors.Add(pd);
                    if (!allDescriptors.Contains(pd)) allDescriptors.Add(pd);
                }
                // add
                AssetType key = (AssetType)Enum.Parse(typeof(AssetType), node.Name.ToString(), true);
                Metadata.Add(key, descriptors.ToArray());
            }
            // register
            var customProvider = new EntityTypeDescriptionProvider(TypeDescriptor.GetProvider(typeof(Entity)),
                new EntityCustomTypeDescriptor(new PropertyDescriptorCollection(allDescriptors.ToArray(), false)));
            TypeDescriptor.AddProvider(customProvider, typeof(Entity));  
        }

        public static T Build<T>(string key, KeyType type)
        {
            return (T)Activator.CreateInstance(typeof(T), Activator.CreateInstance(typeof(EntityKey),key,type));
        }
    }
}

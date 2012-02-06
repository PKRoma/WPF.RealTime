using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using WPF.RealTime.Data;
using WPF.RealTime.Data.Binding;
using WPF.RealTime.Data.Entities;
using WPF.RealTime.Infrastructure.Interfaces;
using System.Linq;

namespace WPF.RealTime.Infrastructure
{
    public abstract class BaseServiceObserver
    {
        private ConcurrentDictionary<ulong, Entity> _entityIndex;
        private readonly int  _od = Convert.ToInt32(ConfigurationManager.AppSettings["OBSERVER_DELAY"]);
        private Action<IEnumerable<Entity>> _eventExploder;

        protected void AddEventExploder(Action<IEnumerable<Entity>> eventExploder)
        {
            _eventExploder = eventExploder;
        }

        public virtual void AddServicesToObserve(IEnumerable<IService> services)
        {
            _entityIndex = new ConcurrentDictionary<ulong, Entity>();

            var observer = services.Select(s => 
                Observable.FromEvent<EventArgs<DataRecord>>(h => s.DataReceived += h, h => s.DataReceived -= h))
                .ToList()
                .Merge();

            observer.Select(x => x.EventArgs.Value)
                    .Where(x => x.DataRecordKey != null)
                    .BufferWithTime(TimeSpan.FromMilliseconds(_od))
                    .Where(x => x.Count > 0)
                    .Subscribe(DataReceived, LogError);
        }

        private static void LogError(Exception ex)
        {
            // log error 
        }

        //buffer
        private void DataReceived(IList<DataRecord> dataRecords)
        {
            ConcurrentDictionary<EntityKey, Entity> buffer = new ConcurrentDictionary<EntityKey, Entity>(); 
            for (int i = 0; i < dataRecords.Count; i++)
            {
                var key = String.Intern(dataRecords[i].DataRecordKey);
                var name = String.Intern(dataRecords[i].PropertyName);
                var value = dataRecords[i].PropertyValue;
                Entity entity;
                bool isNew = !_entityIndex.TryGetValue(EntityBuilder.Fnv1Hash(key), out entity);

                // add 
                if (isNew)
                {
                    entity = EntityBuilder.Build<Entity>(key, KeyType.Isin);
                    _entityIndex.TryAdd(entity.Key.HashedValue, entity);
                }

                // update
                entity.Values.AddOrUpdate(name, new EntityItem(value), (n, oldValue) => new EntityItem(value, oldValue.Value));
                buffer.AddOrUpdate(entity.Key, entity, (n, oldValue) => entity);
            }

            _eventExploder(buffer.Values);
        }
    }
}

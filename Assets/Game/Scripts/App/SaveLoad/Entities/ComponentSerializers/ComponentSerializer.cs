using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Zenject;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    public abstract class ComponentSerializer<TComponent, TData> : IComponentSerializer
        where TComponent : ISerializableComponent
    {
        public Type ComponentType => typeof(TComponent);
        protected virtual string Key => typeof(TComponent).Name;

        public void Serialize(ISerializableComponent component, IDictionary<string, string> saveState)
        {
            var data = Serialize((TComponent)component);
            saveState[Key] = JsonConvert.SerializeObject(data);
        }

        public void Deserialize(ISerializableComponent component, IDictionary<string, string> loadState)
        {
            if (loadState.TryGetValue(Key, out var data))
            {
                Deserialize((TComponent)component, JsonConvert.DeserializeObject<TData>(data));
            }
        }

        public abstract TData Serialize(TComponent component);
        public abstract void Deserialize(TComponent component, TData data);
    }

    public abstract class ComponentSerializer<TComponent, TService, TData> : IComponentSerializer
        where TComponent : ISerializableComponent
    {
        public Type ComponentType => typeof(TComponent);
        protected virtual string Key => typeof(TComponent).Name;

        [Inject] private TService _service;

        public void Serialize(ISerializableComponent component, IDictionary<string, string> saveState)
        {
            var data = Serialize((TComponent)component, _service);
            saveState[Key] = JsonConvert.SerializeObject(data);
        }

        public void Deserialize(ISerializableComponent component, IDictionary<string, string> loadState)
        {
            if (loadState.TryGetValue(Key, out var data))
            {
                Deserialize((TComponent)component, _service, JsonConvert.DeserializeObject<TData>(data));
            }
        }

        public abstract TData Serialize(TComponent component, TService entityCatalog);
        public abstract void Deserialize(TComponent component, TService service, TData data);
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;
using Zenject;

namespace App.SaveLoad.Serializers
{
    public abstract class GameSerializer<TService, TData> : IGameSerializer
    {
        protected virtual string Key => typeof(TData).Name;

        [Inject]
        private TService _service;

        public void Serialize(IDictionary<string, string> saveState)
        {
            TData data = this.Serialize(_service);
            saveState[this.Key] = JsonConvert.SerializeObject(data);
        }

        public void Deserialize(IDictionary<string, string> loadState)
        {
            if (!loadState.TryGetValue(this.Key, out string json))
                return;

            TData data = JsonConvert.DeserializeObject<TData>(json);
            this.Deserialize(_service, data);
        }

        protected abstract TData Serialize(TService service);
        protected abstract void Deserialize(TService service, TData data);
    }
    
    
    public abstract class GameSerializer<TService1, TService2, TData> : IGameSerializer
    {
        protected virtual string Key => typeof(TData).Name;

        [Inject]
        private TService1 _service1;

        [Inject]
        private TService2 _service2;

        public void Serialize(IDictionary<string, string> saveState)
        {
            TData data = this.Serialize(_service1, _service2);
            saveState[this.Key] = JsonConvert.SerializeObject(data);
        }

        public void Deserialize(IDictionary<string, string> loadState)
        {
            if (!loadState.TryGetValue(this.Key, out string json))
                return;

            TData data = JsonConvert.DeserializeObject<TData>(json);
            this.Deserialize(_service1, _service2, data);
        }

        protected abstract TData Serialize(TService1 service1, TService2 service2);
        protected abstract void Deserialize(TService1 service1, TService2 service2, TData data);
    }
    
    
    public abstract class GameSerializer<TService1, TService2, TService3, TData> : IGameSerializer
    {
        protected virtual string Key => typeof(TData).Name;

        [Inject]
        private TService1 _service1;

        [Inject]
        private TService2 _service2;

        [Inject]
        private TService3 _service3;

        public void Serialize(IDictionary<string, string> saveState)
        {
            TData data = this.Serialize(_service1, _service2, _service3);
            saveState[this.Key] = JsonConvert.SerializeObject(data);
        }

        public void Deserialize(IDictionary<string, string> loadState)
        {
            if (!loadState.TryGetValue(this.Key, out string json))
                return;

            TData data = JsonConvert.DeserializeObject<TData>(json);
            this.Deserialize(_service1, _service2, _service3, data);
        }

        protected abstract TData Serialize(TService1 service1, TService2 esh, TService3 service3);
        protected abstract void Deserialize(TService1 service1, TService2 service2, TService3 service3, TData data);
    }
}
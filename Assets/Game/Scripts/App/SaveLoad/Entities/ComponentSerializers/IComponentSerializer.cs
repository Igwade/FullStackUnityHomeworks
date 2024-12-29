using System;
using System.Collections.Generic;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    public interface IComponentSerializer
    {
        public Type ComponentType { get; }
        void Serialize(ISerializableComponent component, IDictionary<string, string> saveState);
        void Deserialize(ISerializableComponent component, IDictionary<string, string> loadState);
    }
}
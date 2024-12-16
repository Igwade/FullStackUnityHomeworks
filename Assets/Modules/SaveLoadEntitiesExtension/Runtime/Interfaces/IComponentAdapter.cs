using UnityEngine;

namespace SaveLoadEntitiesExtension
{
    public interface IComponentAdapter
    {
        string GetComponentIdentifier();
        object GetRawComponent();
    }

    public interface IComponentAdapter<out T> : IComponentAdapter
    {
        new T GetRawComponent();
    }
}
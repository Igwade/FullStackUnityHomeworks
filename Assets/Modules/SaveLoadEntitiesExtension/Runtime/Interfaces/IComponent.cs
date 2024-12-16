using UnityEngine;

namespace SaveLoadEntitiesExtension
{
    public interface IComponent
    {
        string GetComponentIdentifier();
        object GetRawComponent();
    }

    public interface IComponent<out T> : IComponent
    {
        new T GetRawComponent();
    }
}
using UnityEngine;

namespace Modules.ComponentSerialization.Runtime
{
    public interface IComponentSerializer<in TSource, TDto> 
        where TSource : MonoBehaviour 
        where TDto : class
    {
        TDto Serialize(TSource component);
        void Deserialize(TSource component, TDto dto);
    }
}
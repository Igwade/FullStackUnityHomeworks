using SaveLoadEntitiesExtension;

namespace Game.Scripts.SaveLoad
{
    public sealed class ComponentAdapter : IComponent 
    {
        private readonly object _component;

        public ComponentAdapter(object component)
        {
            _component = component;
        }

        public string GetComponentIdentifier() => _component.GetType().Name;
        object IComponent.GetRawComponent() => _component;
    }
}
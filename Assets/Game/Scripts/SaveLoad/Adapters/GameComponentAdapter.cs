using SaveLoadEntitiesExtension;

namespace Game.Scripts.SaveLoad
{
    public sealed class GameComponentAdapter : IComponentAdapter 
    {
        private readonly object _component;

        public GameComponentAdapter(object component)
        {
            _component = component;
        }

        public string GetComponentIdentifier() => _component.GetType().Name;
        object IComponentAdapter.GetRawComponent() => _component;
    }
}
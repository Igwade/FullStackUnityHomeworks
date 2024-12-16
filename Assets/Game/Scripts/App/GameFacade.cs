using Zenject;

namespace SampleGame.App
{
    //Don't Modify
    public sealed class GameFacade
    {
        private DiContainer _current;

        public void SetContainer(DiContainer container) => _current = container;

        public T Resolve<T>() => _current.Resolve<T>();
    }
}
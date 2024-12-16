using SaveLoadEntitiesExtension;
using SampleGame.App;
using SaveLoad;

namespace Game.Scripts.SaveLoad.DataProviders
{
    public abstract class GameDataProvider<T, TV>: IDataProvider<TV>
    {
        public string Key => typeof(T).Name;

        public abstract TV GetData(ISaveLoadContext context);

        public abstract void SetData(TV data, ISaveLoadContext context);

        protected T GetModel(ISaveLoadContext context) 
            => Resolve<T>(context);

        protected TK Resolve<TK>(ISaveLoadContext context) 
            => context.Get<GameFacade>().Resolve<TK>();
    }
}
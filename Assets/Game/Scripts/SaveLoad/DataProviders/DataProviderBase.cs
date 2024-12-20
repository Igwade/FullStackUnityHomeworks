using SampleGame.App;
using SaveLoad;

namespace Game.Scripts.SaveLoad.DataProviders
{
    public abstract class DataProviderBase<TModel, TDto>: IDataProvider<TDto>
    {
        public string Key => typeof(TModel).Name;

        public abstract TDto GetData(ISaveLoadContext context);

        public abstract void SetData(TDto data, ISaveLoadContext context);

        protected TModel GetModel(ISaveLoadContext context) 
            => Resolve<TModel>(context);

        protected TK Resolve<TK>(ISaveLoadContext context) 
            => context.Get<GameFacade>().Resolve<TK>();
    }
}
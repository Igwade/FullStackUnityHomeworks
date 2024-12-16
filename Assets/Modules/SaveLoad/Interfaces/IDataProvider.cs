using SaveLoadEntitiesExtension;

namespace SaveLoad
{
    public interface IDataProvider
    {
        string Key { get; }
        string GetData(ISaveLoadContext context, ISerializer serializer);
        void SetData(string data, ISaveLoadContext context, ISerializer serializer);
    }
    
    public interface IDataProvider<T> : IDataProvider
    {
        T GetData(ISaveLoadContext context);
        void SetData(T data, ISaveLoadContext context);

        string IDataProvider.GetData(ISaveLoadContext context, ISerializer serializer) => serializer.Serialize(GetData(context));
        void IDataProvider.SetData(string data, ISaveLoadContext context, ISerializer serializer) => SetData(serializer.Deserialize<T>(data), context);
    }
}
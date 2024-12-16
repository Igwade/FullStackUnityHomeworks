namespace SaveLoad
{
    public interface ISaveLoadContext
    {
        T Get<T>();
        bool TryGet<T>(out T value);
    }
}
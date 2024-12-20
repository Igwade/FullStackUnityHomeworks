namespace Modules.UnityHttpClient.Interfaces
{
    public interface IBodySerializer
    {
        string ContentType { get; }
        byte[] Serialize<T>(T data);
    }
}
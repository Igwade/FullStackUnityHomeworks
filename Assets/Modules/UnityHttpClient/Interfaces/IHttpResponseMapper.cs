namespace Modules.UnityHttpClient.Interfaces
{
    public interface IHttpResponseMapper<out TResponse>
    {
        TResponse Map(string response);
    }

}
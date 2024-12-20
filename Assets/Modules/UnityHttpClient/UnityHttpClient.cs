using System;
using Cysharp.Threading.Tasks;
using EitherMonad;
using Modules.UnityHttpClient.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

namespace Modules.UnityHttpClient
{
    public class UnityHttpClient : IHttpClient
    {
        public async UniTask<Result<TResponse, string>> SendRequestAsync<TResponse>(HttpRequestConfiguration<TResponse> config)
        {
            Debug.Log(1);

            var attempts = config.RetryCount + 1;

            for (var attempt = 1; attempt <= attempts; attempt++)
            {
                using var webRequest = new UnityWebRequest(config.HttpRequest.Url, config.HttpRequest.Method);

                foreach (var header in config.HttpRequest.Headers) 
                    webRequest.SetRequestHeader(header.Key, header.Value);

                if (config.HttpRequest.Payload is { Length: > 0 }) 
                    webRequest.uploadHandler = new UploadHandlerRaw(config.HttpRequest.Payload);

                webRequest.downloadHandler = new DownloadHandlerBuffer();

                if (config.HttpRequest.Timeout.HasValue) 
                    webRequest.timeout = config.HttpRequest.Timeout.Value;

                Debug.Log(2);
                
                try
                {
                    Debug.Log(3);
                    await webRequest.SendWebRequest().ToUniTask();

                }
                catch (Exception e)
                {

                   Debug.LogError(e);
                    throw;
                }

                Debug.Log(4);

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    var response = config.ResponseMapper.Map(webRequest.downloadHandler.text);
                    return Result<TResponse, string>.FromSuccess(response);
                }
                
                if (attempt < attempts && config.RetryDelayMilliseconds > 0)
                {
                    await UniTask.Delay(config.RetryDelayMilliseconds);
                }

                if (attempt == attempts)
                {
                    return Result<TResponse, string>.FromError($"Error after {attempts} attempts: {webRequest.error}");
                }
            }
            
            return Result<TResponse, string>.FromError("Unknown error");
        }
    }
}

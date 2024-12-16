using System.Text;
using Cysharp.Threading.Tasks;
using EitherMonad;
using SaveLoad;
using UnityEngine.Networking;

namespace Game.Scripts.SaveLoad
{
    public class WebRepository : IRepository
    {
        private const string BaseUrl = "http://localhost:5041";

        public async UniTask<Result<int, string>> Save(int version, string content)
        {
            var url = $"{BaseUrl}/save?version={version}";

            using var webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT);
            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(content);

            var jsonToSend = Encoding.UTF8.GetBytes(jsonContent);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Content-Type", "application/json");

            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                return $"Save failed: {webRequest.error}";
            }

            return version;
        }

        public async UniTask<Result<string, string>> Load(int version)
        {
            var url = $"{BaseUrl}/load?version={version}";

            using var webRequest = UnityWebRequest.Get(url);
            await webRequest.SendWebRequest().ToUniTask();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                return Result<string, string>.FromError($"Load failed: {webRequest.error}");
            }

            return Result<string, string>.FromSuccess(webRequest.downloadHandler.text);
        }

        public async UniTask<Result<int, string>> GetLatestVersion()
        {
            var url = $"{BaseUrl}/latest-version";

            using var webRequest = UnityWebRequest.Get(url);
            await webRequest.SendWebRequest().ToUniTask();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                return $"Failed get latest version: {webRequest.error}";
            }

            if (!int.TryParse(webRequest.downloadHandler.text, out var version))
            {
                return $"Failed to parse latest version: {webRequest.downloadHandler.text}";
            }

            return version;
        }
    }
}
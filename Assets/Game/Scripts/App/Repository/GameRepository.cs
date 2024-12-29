using System;
using System.Collections.Generic;
using System.Threading;
using App.Repository.Middleware;
using App.Repository.Storage;
using Cysharp.Threading.Tasks;
using EitherMonad;
using JetBrains.Annotations;
using Newtonsoft.Json;
using SampleGame.App;

namespace App.Repository
{
    [UsedImplicitly]
    public sealed class GameRepository : IGameRepository
    {
        private const string SaveTimeKey = "SaveTime";
        private static readonly DateTime OriginTime = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly LocalSaveStorage localSaveStorage;
        private readonly RemoteSaveStorage remoteSaveStorage;
        
        private readonly RepositoryMiddlewarePipeline middlewarePipeline;

        public GameRepository(
            LocalSaveStorage localSaveStorage, 
            RemoteSaveStorage remoteSaveStorage, 
            RepositoryMiddlewarePipeline middlewarePipeline)
        {
            this.localSaveStorage = localSaveStorage;
            this.remoteSaveStorage = remoteSaveStorage;
            this.middlewarePipeline = middlewarePipeline;
        }

        public async UniTask<Result<Unit, string>> SetState(
            int version,
            Dictionary<string, string> gameState,
            CancellationToken token = default)
        {
            gameState = await middlewarePipeline.OnBeforeSerialize(gameState);

            var timeSeconds = (DateTime.UtcNow - OriginTime).TotalSeconds.ToString("F0");
            gameState[SaveTimeKey] = timeSeconds;

            var json = JsonConvert.SerializeObject(gameState);

            json = await middlewarePipeline.HandleSaveStringAsync(json);

            var localSave = localSaveStorage.SaveStateAsync(version, json, token);
            var remoteSave = remoteSaveStorage.SaveStateAsync(version, json, token);

            var (localResult, remoteResult) = await UniTask.WhenAll(localSave, remoteSave);

            if (localResult.IsError && remoteResult.IsError)
            {
                return $"Failed to save:\nLocal Error: {localResult.Error}\nRemote Error: {remoteResult.Error}";
            }

            return Unit.Default;
        }

        public async UniTask<Result<Dictionary<string, string>, string>> GetState(int version, CancellationToken token = default)
        {
            var localResult = await localSaveStorage.LoadStateAsync(version, token);
            var remoteResult = await remoteSaveStorage.LoadStateAsync(version, token);

            if (localResult.IsError && remoteResult.IsError)
                return "No valid save data found";

            var localState  = await GetStateFromResultAsync(localResult);
            var remoteState = await GetStateFromResultAsync(remoteResult);

            var localTime  = GetTimestamp(localState);
            var remoteTime = GetTimestamp(remoteState);

            return localTime >= remoteTime ? localState : remoteState;
        }

        private async UniTask<Dictionary<string, string>> GetStateFromResultAsync(Result<string, string> result)
        {
            if (result.IsError)
                return null;

            var transformed = await middlewarePipeline.HandleLoadStringAsync(result.Success);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(transformed);
            dictionary = await middlewarePipeline.OnAfterDeserialize(dictionary);

            return dictionary;
        }

        public async UniTask<Result<int, string>> GetLatestVersion(CancellationToken token = default)
        {
            var localVersionTask  = localSaveStorage.GetLatestVersionAsync(token);
            var remoteVersionTask = remoteSaveStorage.GetLatestVersionAsync(token);

            var (localResult, remoteResult) = await UniTask.WhenAll(localVersionTask, remoteVersionTask);

            if (localResult.IsError && remoteResult.IsError)
                return $"No valid save data found.\n Local Error: {localResult.Error}\n Remote Error: {remoteResult.Error}";

            if (remoteResult.IsSuccess && localResult.IsError)
                return remoteResult;

            if (localResult.IsSuccess && remoteResult.IsError)
                return localResult;

            return Math.Max(localResult.Success, remoteResult.Success);
        }

        private static long GetTimestamp(Dictionary<string, string> state)
        {
            if (state == null)
                return -2;

            if (state.TryGetValue(SaveTimeKey, out var timeString) &&
                long.TryParse(timeString, out var timestamp))
            {
                return timestamp;
            }

            return -1;
        }
    }
}

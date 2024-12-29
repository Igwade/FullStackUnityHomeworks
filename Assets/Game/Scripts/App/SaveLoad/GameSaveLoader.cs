using System;
using System.Collections.Generic;
using App.SaveLoad.Serializers;
using Cysharp.Threading.Tasks;
using EitherMonad;
using SampleGame.App;
using UnityEngine;

namespace App.SaveLoad
{
    public sealed class GameSaveLoader
    {
        private readonly IGameRepository _repository;
        private readonly IEnumerable<IGameSerializer> _serializers;

        public GameSaveLoader(IGameRepository repository, IEnumerable<IGameSerializer> serializers)
        {
            _repository = repository;
            _serializers = serializers;
        }

        public async UniTaskVoid Save(int? version = null, Action<Result<int, string>> callback = null)
        {
            var gameState = CollectGameState();

            if (version.HasValue)
            {
                await SaveWithVersion(version.Value, gameState, callback);
                return;
            }

            await SaveWithLatestVersion(gameState, callback);
        }

        public async UniTaskVoid Load(int? version = null, Action<Result<int, string>> callback = null)
        {
            if (version.HasValue)
            {
                await LoadWithVersion(version.Value, callback);
                return;
            }

            await LoadWithLatestVersion(callback);
        }

        private Dictionary<string, string> CollectGameState()
        {
            var gameState = new Dictionary<string, string>();
            foreach (IGameSerializer serializer in _serializers)
            {
                serializer.Serialize(gameState);
            }

            return gameState;
        }

        private void ApplyGameState(Dictionary<string, string> gameState)
        {
            foreach (IGameSerializer serializer in _serializers)
            {
                serializer.Deserialize(gameState);
            }
        }

        private async UniTask SaveWithVersion(int version, Dictionary<string, string> gameState, Action<Result<int, string>> callback)
        {
            var result = await _repository.SetState(version, gameState);
            callback?.Invoke(result.IsSuccess ? version : $"Failed to save state with specified version: {version}.");
        }

        private async UniTask SaveWithLatestVersion(Dictionary<string, string> gameState, Action<Result<int, string>> callback)
        {
            var newVersion = 1;
            var latestVersionResult = await _repository.GetLatestVersion();

            if (latestVersionResult.IsSuccess)
            {
                newVersion = latestVersionResult.Success + 1;
            }

            var result = await _repository.SetState(newVersion, gameState);
            callback?.Invoke(result.IsSuccess ? newVersion : $"Failed to save state with incremented version.\nError: {result.Error}");
        }

        private async UniTask LoadWithLatestVersion(Action<Result<int, string>> callback)
        {
            var latestVersionResult = await _repository.GetLatestVersion();
            if (latestVersionResult.IsError)
            {
                callback?.Invoke("Could not determine the latest version to load.");
                return;
            }

            await LoadWithVersion(latestVersionResult.Success, callback);
        }

        private async UniTask LoadWithVersion(int version, Action<Result<int, string>> callback)
        {
            (await _repository.GetState(version)).Match(
                onSuccess: state =>
                {
                    ApplyGameState(state);
                    callback?.Invoke(version);
                },
                onError: err => callback?.Invoke($"Failed to load state for version {version}.\n Error: {err}")
            );
        }
    }
}
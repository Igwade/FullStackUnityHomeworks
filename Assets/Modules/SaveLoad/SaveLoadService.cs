using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using EitherMonad;
using SaveLoadEntitiesExtension;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IRepository repository;
        private readonly ISerializer serializer;
        private readonly List<IDataProvider> dataProviders = new();

        public SaveLoadService(IRepository repository, ISerializer serializer)
        {
            this.repository = repository;
            this.serializer = serializer;
        }

        // Можно вынести в SaveLoadRegistry
        public void AddProvider(IDataProvider provider)
        {
            if (dataProviders.Any(p => p.Key == provider.Key))
            {
                Debug.LogWarning($"Provider with key {provider.Key} already exists. Skipping.");
                return;
            }

            dataProviders.Add(provider);
        }

        public async UniTask<Result<int, string>> Save(int? version, ISaveLoadContext context)
        {
            try
            {
                var dataDictionary = dataProviders.ToDictionary(
                    p => p.Key,
                    p => p.GetData(context, serializer)
                );

                var jsonData = JsonConvert.SerializeObject(dataDictionary);

                if (!version.HasValue)
                {
                    var latestVersionResult = await repository.GetLatestVersion();

                    if (latestVersionResult.IsError)
                        return $"Version is not specified, error while trying to get the latest version: {latestVersionResult.Error}";

                    var lastVersion = latestVersionResult.Success;
                    return await repository.Save(lastVersion + 1, jsonData);
                }

                return await repository.Save(version.Value, jsonData);
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        public async UniTask<Result<int, string>> Load(int? version, ISaveLoadContext context)
        {
            try
            {
                if (!version.HasValue)
                {
                    var latestVersion = await repository.GetLatestVersion();
                    if (latestVersion.IsError)
                        return $"Version is not specified, error while trying get latest version: {latestVersion.Error}";

                    version = latestVersion.Success;
                }

                var jsonLoadResult = await repository.Load(version.Value);
                if (jsonLoadResult.IsError)
                    return $"Error while loading save data: {jsonLoadResult.Error}";

                var json = jsonLoadResult.Success;
                if (string.IsNullOrEmpty(json))
                    return $"No save data found for version {version}";

                var dataDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                foreach (var provider in dataProviders)
                {
                    if (dataDictionary.TryGetValue(provider.Key, out var value))
                    {
                        provider.SetData(value, context, serializer);
                    }
                }

                return version;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
﻿using System;
using Cysharp.Threading.Tasks;
using SampleGame.App;
using SaveLoadEntitiesExtension;
using Game.Gameplay;
using Game.Scripts.SaveLoad.Adapters;
using Game.Scripts.SaveLoad.Repositories;
using SaveLoad;
using UnityEngine;

namespace Game.Scripts.SaveLoad
{
    public class GameSaveLoadService
    {
        private readonly GameFacade _gameFacade;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IWorld _world;
        private readonly IRepository _repository;
        private readonly ISerializer _serializer;

        public GameSaveLoadService(GameFacade gameFacade)
        {
            _gameFacade = gameFacade;
            
            _world = new WorldAdapter(gameFacade);
            _repository = gameFacade.Instantiate<WebRepository>(); // new FileRepository("Assets/StreamingAssets/Saves", "save_");
            _serializer = new NewtonsoftJsonSerializer(gameFacade);

            _saveLoadService = new SaveLoadService(_repository, _serializer);
            RegisterDataProvider();
        }

        private void RegisterDataProvider()
        {
            _saveLoadService.AddProvider(new EntitiesDataProvider("Entities", _world));
            _saveLoadService.AddProvider(new WalletDataProvider());
        }

        public async UniTask SaveGame(int? version = null, Action<bool, int> callback = null)
        {
            (await _saveLoadService.Save(version, BuildContext())).MatchAction(
                onSuccess: ver => callback?.Invoke(true, ver),
                onError: error => callback?.Invoke(false, -1)
            );
        }

        public async UniTask LoadGame(int? version = null, Action<bool, int> callback = null)
        {
            (await _saveLoadService.Load(version, BuildContext())).MatchAction(
                onSuccess: ver => callback?.Invoke(true, ver),
                onError: error =>
                {
                    Debug.LogError(error);
                    callback?.Invoke(false, -1);
                });
        }

        private ISaveLoadContext BuildContext()
        {
            var context = new SaveLoadContext();
            context.Register(_gameFacade);
            return context;
        }
    }
}
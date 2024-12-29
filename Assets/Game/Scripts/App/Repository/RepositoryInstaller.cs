using App.Repository;
using App.Repository.Middleware;
using App.Repository.Middleware.Middlewares;
using App.Repository.Storage;
using Modules.UnityHttpClient;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace SampleGame.App
{
    [CreateAssetMenu(
        fileName = "RepositoryInstaller",
        menuName = "Zenject/App/New RepositoryInstaller"
    )]
    public sealed class RepositoryInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private string _fileNamePattern = "save_{0}.json";
        
        [SerializeField]
        private string _latestVersionFileName = "latest_version.txt";

        [SerializeField]
        private string _aesPassword = "123";

        [SerializeField]
        private byte[] _aesSalt = {0x52, 0x41, 0x16, 0x79, 0x86, 0x64, 0x97, 0x22};

        [SerializeField]
        private string _uri = "http://127.0.0.1:8080";
        
        public override void InstallBindings()
        {
            
            InstallWeb();
            InstallStorages();
            InstallMiddleware();
            
            this.Container.BindInterfacesTo<GameRepository>().AsSingle();
        }

        private void InstallWeb()
        {
            this.Container.Bind<UnityHttpClient>().AsSingle();
            this.Container.Bind<GameClient>().AsSingle().WithArguments(_uri);
        }

        private void InstallStorages()
        {
            this.Container.BindInterfacesAndSelfTo<LocalSaveStorage>().AsSingle()
                .WithArguments($"{Application.persistentDataPath}/Saves", _fileNamePattern, _latestVersionFileName);
            this.Container.BindInterfacesAndSelfTo<RemoteSaveStorage>().AsSingle();
        }
        
        private void InstallMiddleware()
        {
            // Порядок важен
            this.Container.BindInterfacesTo<LoggingMiddleware>().AsSingle();
            this.Container.BindInterfacesTo<AesCipherMiddleware>().AsSingle().WithArguments(_aesPassword, _aesSalt);


            this.Container.BindInterfacesAndSelfTo<RepositoryMiddlewarePipeline>().AsSingle();
        }
    }
}
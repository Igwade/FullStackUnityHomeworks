using App.Repository;
using App.Repository.ChainOfResponsibility;
using App.Repository.ChainOfResponsibility.GetLatestVersion;
using App.Repository.ChainOfResponsibility.GetLatestVersion.Handlers;
using App.Repository.ChainOfResponsibility.GetState;
using App.Repository.ChainOfResponsibility.GetState.Handlers;
using App.Repository.ChainOfResponsibility.SetState;
using App.Repository.ChainOfResponsibility.SetState.Handlers;
using App.Repository.Storage;
using Modules.UnityHttpClient;
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
        [SerializeField] private string _fileNamePattern = "save_{0}.json";

        [SerializeField] private string _latestVersionFileName = "latest_version.txt";

        [SerializeField] private string _aesPassword = "123";

        [SerializeField] private byte[] _aesSalt = { 0x52, 0x41, 0x16, 0x79, 0x86, 0x64, 0x97, 0x22 };

        [SerializeField] private string _uri = "http://127.0.0.1:8080";

        public override void InstallBindings()
        {
            InstallWeb();
            InstallLocalSaveStorage();
            InstallChainOfResponsibility();

            this.Container.BindInterfacesTo<GameRepository>().AsSingle();
        }

        private void InstallLocalSaveStorage()
        {
            this.Container.BindInterfacesAndSelfTo<LocalSaveStorage>().AsSingle()
                .WithArguments($"{Application.persistentDataPath}/Saves", _fileNamePattern, _latestVersionFileName);
        }

        private void InstallWeb()
        {
            this.Container.Bind<UnityHttpClient>().AsSingle();
            this.Container.Bind<GameClient>().AsSingle().WithArguments(_uri);
        }

        private void InstallChainOfResponsibility()
        {
            InstallSetStateChain();
            InstallGetStateChain();
            InstallGetLatestVersionChain();
        }

        private void InstallSetStateChain()
        {
            var addTimestamp = Container.Instantiate<AddTimestampHandler>();
            var serialize = Container.Instantiate<SerializeHandler>();
            var aesEncryption = Container.Instantiate<AesEncryptionHandler>(new object[] { _aesPassword, _aesSalt });
            var localSave = Container.Instantiate<LocalSaveHandler>();
            var remoteSave = Container.Instantiate<RemoteSaveHandler>();
            var evaluateSave = Container.Instantiate<EvaluateSaveHandler>();

            addTimestamp
                .SetNext(serialize)
                .SetNext(aesEncryption)
                .SetNext(localSave)
                .SetNext(remoteSave)
                .SetNext(evaluateSave);

            Container.Bind<IHandler<SetStateContext>>()
                .FromInstance(addTimestamp)
                .AsSingle();
        }

        private void InstallGetStateChain()
        {
            var localLoad = Container.Instantiate<LocalLoadHandler>();
            var remoteLoad = Container.Instantiate<RemoteLoadHandler>();
            var evaluateLoad = Container.Instantiate<EvaluateLoadHandler>();
            var compareTimestamps = Container.Instantiate<CompareTimestampsHandler>();
            var aesDecryption = Container.Instantiate<AesDecryptionHandler>(new object[] { _aesPassword, _aesSalt });
            var deserialize = Container.Instantiate<DeserializeHandler>();

            localLoad
                .SetNext(remoteLoad)
                .SetNext(evaluateLoad)
                .SetNext(aesDecryption)
                .SetNext(deserialize)
                .SetNext(compareTimestamps);

            Container.Bind<IHandler<GetStateContext>>()
                .FromInstance(localLoad)
                .AsSingle();
        }

        private void InstallGetLatestVersionChain()
        {
            var localVersion = Container.Instantiate<LocalVersionHandler>();
            var remoteVersion = Container.Instantiate<RemoteVersionHandler>();
            var evaluateVersion = Container.Instantiate<EvaluateVersionHandler>();

            localVersion
                .SetNext(remoteVersion)
                .SetNext(evaluateVersion);

            Container.Bind<IHandler<GetLatestVersionContext>>()
                .FromInstance(localVersion)
                .AsSingle();
        }
    }
}
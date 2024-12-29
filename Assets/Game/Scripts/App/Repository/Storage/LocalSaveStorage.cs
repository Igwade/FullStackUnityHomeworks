using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EitherMonad;
using JetBrains.Annotations;

namespace App.Repository.Storage
{
    [UsedImplicitly]
    public sealed class LocalSaveStorage : ISaveStorage
    {
        private readonly string folderPath;
        private readonly string fileNamePattern;

        private string VersionPath(int version) => Path.Combine(folderPath, string.Format(fileNamePattern, version));
        private string LatestVersionPath => Path.Combine(folderPath, latestVersionFileName);
        
        
        private readonly string latestVersionFileName;

        public LocalSaveStorage(string folderPath, string fileNamePattern, string latestVersionFileName)
        {
            this.folderPath = folderPath;
            this.fileNamePattern = fileNamePattern;
            this.latestVersionFileName = latestVersionFileName;
        }

        public async UniTask<Result<Unit, string>> SaveStateAsync(int version, string state, CancellationToken token = default)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = VersionPath(version);

                await File.WriteAllTextAsync(filePath, state, token);
                await UpdateLatestVersion(version, token);

                return Unit.Default;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private async Task UpdateLatestVersion(int version, CancellationToken token)
        {
            var currentLatestVersion = await GetLatestVersionAsync(token);
                
            var latestVersion = currentLatestVersion.IsSuccess
                ? Math.Max(version, currentLatestVersion.Success)
                : version;
                
            await File.WriteAllTextAsync(LatestVersionPath, latestVersion.ToString(), token);
        }

        public async UniTask<Result<int, string>> GetLatestVersionAsync(CancellationToken token = default)
        {
            try
            {
                if (!File.Exists(LatestVersionPath))
                    return "File not found";

                var content = await File.ReadAllTextAsync(LatestVersionPath, token);

                if (string.IsNullOrWhiteSpace(content))
                    return "File is empty";

                if (!int.TryParse(content, out var version))
                    return "Invalid version format";

                return version;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async UniTask<Result<string, string>> LoadStateAsync(int version, CancellationToken token = default)
        {
            try
            {
                var filePath = VersionPath(version);

                if (!File.Exists(filePath))
                    return Result<string, string>.FromError("File not found");

                var content = await File.ReadAllTextAsync(filePath, token);

                if (string.IsNullOrWhiteSpace(content))
                    return Result<string, string>.FromError("File is empty");

                return Result<string, string>.FromSuccess(content);
            }
            catch (Exception e)
            {
                return Result<string, string>.FromError(e.Message);
            }
        }
    }
}
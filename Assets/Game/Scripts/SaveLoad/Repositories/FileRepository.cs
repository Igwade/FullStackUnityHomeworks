using System;
using System.IO;
using Cysharp.Threading.Tasks;
using EitherMonad;
using SaveLoad;

namespace Game.Scripts.SaveLoad
{
    public sealed class FileRepository : IRepository
    {
        private readonly string _path;
        private readonly string _fileNamePrefix;

        public FileRepository(string path, string fileNamePrefix)
        {
            this._path = path;
            this._fileNamePrefix = fileNamePrefix;
        }

        public async UniTask<Result<int, string>> Save(int version, string data)
        {
            try
            {
                var directoryPath = Path.Combine(_path);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, $"{_fileNamePrefix}{version}");

                await File.WriteAllTextAsync(filePath, data);
                return version;
            }
            catch (Exception e)
            {
                return $"Failed to save file: {e.Message}";
            }
        }

        public async UniTask<Result<string, string>> Load(int version)
        {
            try
            {
                var json = await File.ReadAllTextAsync(Path.Combine(_path, $"{_fileNamePrefix}{version}"));
                return Result<string, string>.FromSuccess(json);
            }
            catch (Exception e)
            {
                return Result<string, string>.FromError($"Failed to load file: {e.Message}");
            }
        }

        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async UniTask<Result<int, string>> GetLatestVersion()
        {
            try
            {
                var directoryPath = Path.Combine(_path);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                    return 0;
                }

                var files = Directory.GetFiles(directoryPath, $"{_fileNamePrefix}*");

                var maxVersion = 0;
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    if (fileName.StartsWith(_fileNamePrefix))
                    {
                        var versionPart = fileName[_fileNamePrefix.Length..];
                        if (int.TryParse(versionPart, out var version))
                        {
                            maxVersion = Math.Max(maxVersion, version);
                        }
                    }
                }

                return maxVersion;
            }
            catch (Exception e)
            {
                return $"Failed to get latest version: {e.Message}";
            }
        }
    }
}
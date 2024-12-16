using System.Collections.Generic;
using SaveLoad;

namespace SaveLoadEntitiesExtension
{
    public delegate string GetSaveDataDelegate(IComponent comp, ISerializer serializer, ISaveLoadContext context);
    public delegate void LoadFromDataDelegate(IComponent comp, string json, ISerializer serializer, ISaveLoadContext context);

    public static class SaveableComponentRegistry
    {
        private static readonly Dictionary<string, (GetSaveDataDelegate save, LoadFromDataDelegate load)> Registry = new();

        public static void Register(string componentTypeName, GetSaveDataDelegate save, LoadFromDataDelegate load)
        {
            Registry[componentTypeName] = (save, load);
        }

        public static bool TryGet(string componentTypeName, out (GetSaveDataDelegate save, LoadFromDataDelegate load) entry)
        {
            return Registry.TryGetValue(componentTypeName, out entry);
        }
    }
}
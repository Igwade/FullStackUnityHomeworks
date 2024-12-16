using System.Linq;
using JetBrains.Annotations;
using Modules.Entities;
using SampleGame.App;
using SaveLoadEntitiesExtension;
using UnityEngine;

namespace Game.Scripts.SaveLoad
{
    [UsedImplicitly]
    public sealed class GameWorldAdapter : IWorldAdapter
    {
        private EntityWorld World => _gameFacade.Resolve<EntityWorld>();

        private readonly GameFacade _gameFacade;

        public GameWorldAdapter(GameFacade gameFacade)
        {
            _gameFacade = gameFacade;
        }

        public void DestroyAllEntities() => World.DestroyAll();

        public IEntityAdapter[] GetAllEntities() =>
            World.GetAll().Select(e => new GameEntityAdapter(e)).ToArray<IEntityAdapter>();

        public IEntityAdapter SpawnEntity(string entityName, float x, float y, float z, float rx, float ry, float rz,
            int id = -1)
        {
            var entity = World.Spawn(entityName, new Vector3(x, y, z), Quaternion.Euler(rx, ry, rz), id);
            return new GameEntityAdapter(entity);
        }
    }
}
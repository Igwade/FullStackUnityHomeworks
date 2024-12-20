using System.Linq;
using JetBrains.Annotations;
using Modules.Entities;
using SampleGame.App;
using SaveLoadEntitiesExtension;
using UnityEngine;

namespace Game.Scripts.SaveLoad.Adapters
{
    [UsedImplicitly]
    public sealed class WorldAdapter : IWorld
    {
        private EntityWorld World => _gameFacade.Resolve<EntityWorld>();

        private readonly GameFacade _gameFacade;

        public WorldAdapter(GameFacade gameFacade)
        {
            _gameFacade = gameFacade;
        }

        public void DestroyAllEntities() => World.DestroyAll();

        public IEntity[] GetAllEntities() =>
            World.GetAll().Select(e => new EntityAdapter(e)).ToArray<IEntity>();

        public IEntity SpawnEntity(string entityName, float x, float y, float z, float rx, float ry, float rz,
            int id = -1)
        {
            var entity = World.Spawn(entityName, new Vector3(x, y, z), Quaternion.Euler(rx, ry, rz), id);
            return new EntityAdapter(entity);
        }
    }
}
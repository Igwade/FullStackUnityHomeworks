using System.Linq;
using Modules.Entities;
using SaveLoadEntitiesExtension;
using UnityEngine;

namespace Game.Scripts.SaveLoad.Adapters
{
    public sealed class EntityAdapter : IEntity
    {
        private readonly Entity _entity;
        public EntityAdapter(Entity entity) { _entity = entity; }

        public int GetId() => _entity.Id;

        public string GetName() => _entity.Name;

        public string GetEntityType() => _entity.Type.ToString();

        public (float x, float y, float z) GetPosition()
        {
            var p = _entity.transform.position;
            return (p.x, p.y, p.z);
        }

        public (float x, float y, float z) GetRotation()
        {
            var r = _entity.transform.rotation.eulerAngles;
            return (r.x, r.y, r.z);
        }

        public void SetPosition(float x, float y, float z)
        {
            _entity.transform.position = new Vector3(x,y,z);
        }

        public void SetRotation(float x, float y, float z)
        {
            _entity.transform.rotation = Quaternion.Euler(x,y,z);
        }

        public IComponent[] GetComponents()
        {
            var comps = _entity.GetComponents<MonoBehaviour>();
            return comps.Select(c => new ComponentAdapter(c)).ToArray<IComponent>();
        }
    }
}
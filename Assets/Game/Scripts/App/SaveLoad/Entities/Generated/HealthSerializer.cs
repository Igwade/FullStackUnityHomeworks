using UnityEngine;
using System;
using System.Collections.Generic;

// Auto-generated by SaveCodeGenerator
namespace Modules.ComponentSerialization
{
    // ----- Health -----

    [Serializable]
    public class HealthDto
    {
        public Int32 Current;
    }

    public static class HealthSerializer
    {
        public static HealthDto Serialize(Game.Scripts.Gameplay.Components.Health source)
        {
            var dto = new HealthDto();
            dto.Current = source.Current;
            return dto;
        }

        public static void Deserialize(Game.Scripts.Gameplay.Components.Health target, HealthDto dto)
        {
            target.Current = dto.Current;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Register()
        {
            ComponentSerializersRegistry.Register(typeof(Game.Scripts.Gameplay.Components.Health), new ComponentSerializer
            {
                DtoType = typeof(HealthDto),
                Serialize = (mono) => Serialize((Game.Scripts.Gameplay.Components.Health)mono),
                Deserialize = (mono, dto) => Deserialize((Game.Scripts.Gameplay.Components.Health)mono, (HealthDto)dto)
            });
        }
    }

}
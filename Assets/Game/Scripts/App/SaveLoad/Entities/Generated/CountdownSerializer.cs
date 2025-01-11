using UnityEngine;
using System;
using System.Collections.Generic;

// Auto-generated by SaveCodeGenerator
namespace Modules.ComponentSerialization
{
    // ----- Countdown -----

    [Serializable]
    public class CountdownDto
    {
        public Single Current;
    }

    public static class CountdownSerializer
    {
        public static CountdownDto Serialize(SampleGame.Gameplay.Countdown source)
        {
            var dto = new CountdownDto();
            dto.Current = source.Current;
            return dto;
        }

        public static void Deserialize(SampleGame.Gameplay.Countdown target, CountdownDto dto)
        {
            target.Current = dto.Current;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Register()
        {
            ComponentSerializersRegistry.Register(typeof(SampleGame.Gameplay.Countdown), new ComponentSerializer
            {
                DtoType = typeof(CountdownDto),
                Serialize = (mono) => Serialize((SampleGame.Gameplay.Countdown)mono),
                Deserialize = (mono, dto) => Deserialize((SampleGame.Gameplay.Countdown)mono, (CountdownDto)dto)
            });
        }
    }

}
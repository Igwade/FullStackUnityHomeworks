using System;
using Gameplay.Components;
using Modules.ComponentSerialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    [Serializable]
    public class TestDto
    {
        public int value;
    }

    // Не обязательно быть статичным. Это может быть обычный объект, главное сделать регистрацию
    public static class TestSerializer
    {
        private static TestDto Serialize(TestComponent source) =>
            new()
            {
                value = source.Value
            };

        private static void Deserialize(TestComponent target, TestDto dto)
        {
            target.Value = dto.value + Random.Range(-100, 100);
        }

        // Можно регистрировать сериализаторы вручную откуда угодно, тут для удобства так
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Register()
        {
            ComponentSerializersRegistry.Register(typeof(TestComponent), new ComponentSerializer
            {
                DtoType = typeof(TestDto),
                Serialize = (mono) => Serialize((TestComponent)mono),
                Deserialize = (mono, dto) => Deserialize((TestComponent)mono, (TestDto)dto)
            });
        }
    }
}
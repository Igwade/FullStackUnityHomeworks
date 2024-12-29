using System;
using Game.Scripts.Gameplay.Components;
using JetBrains.Annotations;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    // Пока не понял зачем разделять данные и сериализаторы, по мне удобней когда они рядом
    // Ведь всё равно нет места где бы мы могли на них все сразу посмотреть
    [Serializable]
    public class HealthData
    {
        public int current;
    }
    
    [UsedImplicitly]
    public class HealthSerializer : ComponentSerializer<Health, HealthData>
    {
        public override HealthData Serialize(Health component) => new()
        {
            current = component.Current
        };

        public override void Deserialize(Health component, HealthData data)
        {
            component.Current = data.current;
        }
    }
}
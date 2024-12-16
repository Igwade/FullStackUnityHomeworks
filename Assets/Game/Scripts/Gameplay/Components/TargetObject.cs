using Modules.Entities;
using SaveLoadEntitiesExtension.Attributes;
using UnityEngine;

namespace SampleGame.Gameplay
{
    [SaveComponent]
    // [UseCustomSerializer(typeof(TargetObjectCustomSerializer))]
    public sealed class TargetObject : MonoBehaviour
    {
        ///Variable
        [field: SerializeField, Saveable]
        public Entity Value { get; set; }
    }
    
    // public sealed partial class TargetObject
    // {
    //     public sealed class TargetObjectCustomSerializer
    //     {
    //         public static string Serialize(IComponentAdapter component, ISerializer serializer, ISerializationContext context)
    //         {
    //             var targetObject = (TargetObject) component.GetRawComponent();
    //             return serializer.Serialize(targetObject!.Value?.Id);
    //         }
    //     
    //         public static void Deserialize(IComponentAdapter component, string json, ISerializer serializer, ISerializationContext context)
    //         {
    //             if (!context.TryGet<EntityWorld>(out var world))
    //             {
    //                 throw new System.Exception("No EntityWorld found in context");
    //             }
    //      
    //             var targetObject = component.GetRawComponent() as TargetObject;
    //             var targetId = serializer.Deserialize<int?>(json);
    //         
    //             if(targetId == null)
    //             {
    //                 targetObject!.Value = null;
    //                 return;
    //             }
    //         
    //             if(world.TryGet(targetId.Value, out var entity))
    //             {
    //                 targetObject!.Value = entity;
    //             }
    //         }
    //     }
    // }
}
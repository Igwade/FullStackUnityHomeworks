using System;
using Modules.ComponentSerialization.Runtime;
using UnityEngine;

namespace App.SaveLoad.Entities.TypeSerializers
{
    
    [Serializable]
    public struct Vector3Dto
    {
        public float x, y, z;
    }

    public class Vector3Serializer : ITypeSerializer<Vector3, Vector3Dto>
    {
        public Vector3Dto Serialize(Vector3 value)
        {
            return new Vector3Dto { x = value.x, y = value.y, z = value.z };
        }

        public Vector3 Deserialize(Vector3Dto dto)
        {
            return new Vector3(dto.x, dto.y, dto.z);
        }
    }
}
using System;
using System.Collections.Generic;

namespace SaveLoadEntitiesExtension.Dtos
{
    [Serializable]
    public class EntityData
    {
        public int id;
        public string entityName;
        public string entityType;
        public float px, py, pz;
        public float rx, ry, rz;
        public Dictionary<string, string> Components;
    }
}
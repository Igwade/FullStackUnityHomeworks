using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Scripts.SaveLoad.CustomConverters
{
    public class Vector3JsonConverter : CustomJsonConverter<Vector3>
    {
        protected override void WriteProperties(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
        }

        protected override Vector3 ReadProperties(JsonReader reader, JsonSerializer serializer)
        {
            var x = 0f;
            var y = 0f;
            var z = 0f;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = reader.Value!.ToString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "x":
                            x = Convert.ToSingle(reader.Value);
                            break;
                        case "y":
                            y = Convert.ToSingle(reader.Value);
                            break;
                        case "z":
                            z = Convert.ToSingle(reader.Value);
                            break;
                    }
                }
            }

            return new Vector3(x, y, z);
        }
    }
}
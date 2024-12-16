using System;
using Newtonsoft.Json;

namespace Game.Scripts.SaveLoad.CustomConverters
{
    public abstract class CustomJsonConverter<T> : JsonConverter<T>
    {
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            WriteProperties(writer, value, serializer);
            writer.WriteEndObject();
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return default;

            return ReadProperties(reader, serializer);
        }

        protected abstract void WriteProperties(JsonWriter writer, T value, JsonSerializer serializer);
        protected abstract T ReadProperties(JsonReader reader, JsonSerializer serializer);
    }
}
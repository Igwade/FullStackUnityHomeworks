using System;
using Modules.Entities;
using Newtonsoft.Json;
using SampleGame.App;

namespace Game.Scripts.SaveLoad.CustomConverters
{
    public class EntityJsonConverter : CustomJsonConverter<Entity>
    {
        private readonly GameFacade _gameFacade;

        public EntityJsonConverter(GameFacade gameFacade)
        {
            this._gameFacade = gameFacade;
        }

        protected override void WriteProperties(JsonWriter writer, Entity value, JsonSerializer serializer)
        {
            writer.WritePropertyName("id");
            writer.WriteValue(value.Id);
        }

        protected override Entity ReadProperties(JsonReader reader, JsonSerializer serializer)
        {
            int id = -1;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = (string)reader.Value;

                    if (reader.Read() && propertyName == "id")
                    {
                        id = reader.Value != null ? Convert.ToInt32(reader.Value) : -1;
                    }
                }
            }

            var world = _gameFacade.Resolve<EntityWorld>();

            if (world.TryGet(id, out var entity))
            {
                return entity;
            }

            return null;
        }
    }
}
using System;
using Modules.Entities;
using Newtonsoft.Json;
using SampleGame.App;

namespace Game.Scripts.SaveLoad.CustomConverters
{
    public class EntityConfigJsonConverter : CustomJsonConverter<EntityConfig>
    {
        private readonly GameFacade _gameFacade;

        public EntityConfigJsonConverter(GameFacade gameFacade)
        {
            this._gameFacade = gameFacade;
        }

        protected override void WriteProperties(JsonWriter writer, EntityConfig value, JsonSerializer serializer)
        {
            writer.WritePropertyName("name");
            writer.WriteValue(value.Name);
        }

        protected override EntityConfig ReadProperties(JsonReader reader, JsonSerializer serializer)
        {
            string name = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = (string)reader.Value;

                    if (reader.Read() && propertyName == "name")
                    {
                        name = reader.Value?.ToString();
                    }
                }
            }

            var entityCatalog = _gameFacade.Resolve<EntityCatalog>();
            return entityCatalog.FindConfig(name, out var config) ? config : null;
        }
    }
}
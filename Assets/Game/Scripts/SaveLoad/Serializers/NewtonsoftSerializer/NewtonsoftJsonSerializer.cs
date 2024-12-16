using Game.Scripts.SaveLoad.CustomConverters;
using SaveLoadEntitiesExtension;
using Newtonsoft.Json;
using SampleGame.App;

namespace Game.Scripts.SaveLoad
{
    public sealed class NewtonsoftJsonSerializer : ISerializer
    {
        private readonly GameFacade _gameFacade;
        private readonly JsonSerializerSettings _settings;
        
        public NewtonsoftJsonSerializer(GameFacade gameFacade)
        {
            this._gameFacade = gameFacade;
            this._settings = new JsonSerializerSettings();

            RegisterConverters(_settings);
        }

        private JsonSerializerSettings RegisterConverters(JsonSerializerSettings settings)
        {
            settings.Converters.Add(new EntityConfigJsonConverter(_gameFacade));
            settings.Converters.Add(new EntityJsonConverter(_gameFacade));
            settings.Converters.Add(new Vector3JsonConverter());

            return settings;
        }
        
        public string Serialize(object obj) => JsonConvert.SerializeObject(obj, _settings);
        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, _settings);
    }
}
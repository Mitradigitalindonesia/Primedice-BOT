using Newtonsoft.Json;
using System;

namespace KriPod.Primedice.Converters
{
    class ServerSeedConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            return value != null ? new ServerSeed(value.ToString()) : null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((ServerSeed)value).HexString);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ServerSeed);
        }
    }
}

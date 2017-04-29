using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace KriPod.Primedice.Converters
{
    class BetConditionConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.Value?.ToString()) {
                case "<":
                    return BetCondition.LowerThan;

                case ">":
                    return BetCondition.GreaterThan;

                default:
                    return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jsonString = ((BetCondition)value).ToJsonString();
            if (jsonString != null) {
                writer.WriteValue(jsonString);
            }
        }
    }
}

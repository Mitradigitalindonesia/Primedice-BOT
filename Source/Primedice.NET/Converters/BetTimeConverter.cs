using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace KriPod.Primedice.Converters
{
    class BetTimeConverter : DateTimeConverterBase
    {
        private const string Format = "ddd MMM dd yyyy HH:mm:ss GMT+0000 (UTC)";

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();
            if (value == null) return null;

            // Try parsing the time regularly
            DateTime dateTime;
            if (!DateTime.TryParse(value, out dateTime)) {
                // Proceed with special parsing if necessary
                dateTime = DateTime.ParseExact(value, Format, CultureInfo.InvariantCulture);
            }

            // Return the output as a UTC timestamp
            return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}

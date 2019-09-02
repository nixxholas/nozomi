using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nozomi.Base.Core.Helpers.JSON
{
    public class SerializationConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var list = value as List<KeyValuePair<string, object>>;
            writer.WriteStartArray();
            if (list != null)
                foreach (var item in list)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName(item.Key);
                    // Needed because of the dynamic object.
                    var jsonValue = JsonConvert.SerializeObject(item.Value);
                    writer.WriteValue(jsonValue);
                    writer.WriteEndObject();
                }

            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<KeyValuePair<string, object>>);
        }
    }
}